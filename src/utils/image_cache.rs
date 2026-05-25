// use std::{collections::HashMap, sync::Arc};
// use futures::FutureExt;
// use gpui::{App, AppContext, Asset, AssetLogger, Context, ElementId, Entity, ImageAssetLoader, ImageCache, ImageCacheProvider, Window, hash};


// // TAKEN FROM GPUI EXAMPLES
// pub fn simple_lru_cache(id: impl Into<ElementId>, max_items: usize) -> SimpleLruCacheProvider {
//     SimpleLruCacheProvider {
//         id: id.into(),
//         max_items,
//     }
// }

// pub struct SimpleLruCacheProvider {
//     id: ElementId,
//     max_items: usize,
// }

// impl ImageCacheProvider for SimpleLruCacheProvider {
//     fn provide(&mut self, window: &mut Window, cx: &mut App) -> gpui::AnyImageCache {
//         window
//             .with_global_id(self.id.clone(), |global_id, window| {
//                 window.with_element_state::<Entity<SimpleLruCache>, _>(
//                     global_id,
//                     |lru_cache, _window| {
//                         let mut lru_cache = lru_cache.unwrap_or_else(|| {
//                             cx.new(|cx| SimpleLruCache::new(self.max_items, cx))
//                         });
//                         if lru_cache.read(cx).max_items != self.max_items {
//                             lru_cache = cx.new(|cx| SimpleLruCache::new(self.max_items, cx));
//                         }
//                         (lru_cache.clone(), lru_cache)
//                     },
//                 )
//             })
//             .into()
//     }
// }

// struct SimpleLruCache {
//     max_items: usize,
//     usages: Vec<u64>,
//     cache: HashMap<u64, gpui::ImageCacheItem>,
// }

// impl SimpleLruCache {
//     fn new(max_items: usize, cx: &mut Context<Self>) -> Self {
//         cx.on_release(|simple_cache, cx| {
//             for (_, mut item) in std::mem::take(&mut simple_cache.cache) {
//                 if let Some(Ok(image)) = item.get() {
//                     cx.drop_image(image, None);
//                 }
//             }
//         })
//         .detach();

//         Self {
//             max_items,
//             usages: Vec::with_capacity(max_items),
//             cache: HashMap::with_capacity(max_items),
//         }
//     }
// }

// impl ImageCache for SimpleLruCache {
//     fn load(
//         &mut self,
//         resource: &gpui::Resource,
//         window: &mut Window,
//         cx: &mut App,
//     ) -> Option<Result<Arc<gpui::RenderImage>, gpui::ImageCacheError>> {
//         assert_eq!(self.usages.len(), self.cache.len());
//         assert!(self.cache.len() <= self.max_items);

//         let hash = hash(resource);

//         if let Some(item) = self.cache.get_mut(&hash) {
//             let current_ix = self
//                 .usages
//                 .iter()
//                 .position(|item| *item == hash)
//                 .expect("cache and usages must stay in sync");
//             self.usages.remove(current_ix);
//             self.usages.insert(0, hash);

//             return item.get();
//         }

//         let fut = AssetLogger::<ImageAssetLoader>::load(resource.clone(), cx);
//         let task = cx.background_executor().spawn(fut).shared();
//         if self.usages.len() == self.max_items {
//             let oldest = self.usages.pop().unwrap();
//             let mut image = self
//                 .cache
//                 .remove(&oldest)
//                 .expect("cache and usages must be in sync");
//             if let Some(Ok(image)) = image.get() {
//                 cx.drop_image(image, Some(window));
//             }
//         }
//         self.cache
//             .insert(hash, gpui::ImageCacheItem::Loading(task.clone()));
//         self.usages.insert(0, hash);

//         let entity = window.current_view();
//         window
//             .spawn(cx, {
//                 async move |cx| {
//                     _ = task.await;
//                     cx.on_next_frame(move |_, cx| {
//                         cx.notify(entity);
//                     });
//                 }
//             })
//             .detach();

//         None
//     }
// }


use std::{
    collections::HashMap,
    sync::{Arc, Weak},
};

use futures::FutureExt;
use gpui::{
    App, AppContext, Asset, AssetLogger, Context, ElementId, Entity, ImageAssetLoader, ImageCache, ImageCacheError, ImageCacheProvider, RenderImage, Window, hash
};

pub fn simple_lru_cache(
    id: impl Into<ElementId>,
    max_bytes: usize,
) -> SimpleLruCacheProvider {
    SimpleLruCacheProvider {
        id: id.into(),
        max_bytes,
    }
}

pub struct SimpleLruCacheProvider {
    id: ElementId,
    max_bytes: usize,
}

impl ImageCacheProvider for SimpleLruCacheProvider {
    fn provide(&mut self, window: &mut Window, cx: &mut App) -> gpui::AnyImageCache {
        window
            .with_global_id(self.id.clone(), |global_id, window| {
                window.with_element_state::<Entity<SimpleLruCache>, _>(
                    global_id,
                    |cache, _window| {
                        let mut cache = cache.unwrap_or_else(|| {
                            cx.new(|cx| SimpleLruCache::new(self.max_bytes, cx))
                        });

                        if cache.read(cx).max_bytes != self.max_bytes {
                            cache = cx.new(|cx| {
                                SimpleLruCache::new(self.max_bytes, cx)
                            });
                        }

                        (cache.clone(), cache)
                    },
                )
            })
            .into()
    }
}

struct CacheEntry {
    image: Weak<RenderImage>,
    bytes: usize,
}

struct SimpleLruCache {
    max_bytes: usize,
    current_bytes: usize,

    // newest -> oldest
    usages: Vec<u64>,

    cache: HashMap<u64, CacheEntry>,

    loading: HashMap<
        u64,
        futures::future::Shared<
            gpui::Task<Result<Arc<RenderImage>, ImageCacheError>>,
        >,
    >,
}

impl SimpleLruCache {
    fn new(max_bytes: usize, cx: &mut Context<Self>) -> Self {
        cx.on_release(|cache, cx| {
            for (_, entry) in std::mem::take(&mut cache.cache) {
                if let Some(image) = entry.image.upgrade() {
                    cx.drop_image(image, None);
                }
            }
        })
        .detach();

        Self {
            max_bytes,
            current_bytes: 0,
            usages: Vec::new(),
            cache: HashMap::new(),
            loading: HashMap::new(),
        }
    }

    fn estimate_size(image: &RenderImage) -> usize {
        let size = image.size(0);

        size.width.0 as usize
            * size.height.0 as usize
            * 4 // RGBA8
    }

    fn touch(&mut self, hash: u64) {
        if let Some(ix) = self.usages.iter().position(|v| *v == hash) {
            self.usages.remove(ix);
        }

        self.usages.insert(0, hash);
    }

    fn evict_until_fit(&mut self, needed: usize, window: &mut Window, cx: &mut App) {
        while self.current_bytes + needed > self.max_bytes {
            let Some(oldest) = self.usages.pop() else {
                break;
            };

            if let Some(entry) = self.cache.remove(&oldest) {
                self.current_bytes =
                    self.current_bytes.saturating_sub(entry.bytes);

                if let Some(image) = entry.image.upgrade() {
                    cx.drop_image(image, Some(window));
                }
            }
        }
    }

    fn remove_dead_weak_refs(&mut self) {
        let dead: Vec<u64> = self
            .cache
            .iter()
            .filter_map(|(k, v)| {
                if v.image.strong_count() == 0 {
                    Some(*k)
                } else {
                    None
                }
            })
            .collect();

        for key in dead {
            if let Some(entry) = self.cache.remove(&key) {
                self.current_bytes =
                    self.current_bytes.saturating_sub(entry.bytes);
            }

            self.usages.retain(|v| *v != key);
        }
    }
}

impl ImageCache for SimpleLruCache {
    fn load(
        &mut self,
        resource: &gpui::Resource,
        window: &mut Window,
        cx: &mut App,
    ) -> Option<Result<Arc<RenderImage>, ImageCacheError>> {
        self.remove_dead_weak_refs();

        let hash = hash(resource);

        //
        // already cached
        //
        if let Some(entry) = self.cache.get(&hash) {
            if let Some(image) = entry.image.upgrade() {
                self.touch(hash);
                return Some(Ok(image));
            }
        }

        //
        // currently loading
        //
        if let Some(task) = self.loading.get(&hash) {
            if let Some(result) = task.clone().now_or_never() {
                self.loading.remove(&hash);

                if let Ok(image) = &result {
                    let bytes = Self::estimate_size(image);

                    self.evict_until_fit(bytes, window, cx);

                    self.current_bytes += bytes;

                    self.cache.insert(
                        hash,
                        CacheEntry {
                            image: Arc::downgrade(image),
                            bytes,
                        },
                    );

                    self.touch(hash);
                }

                return Some(result);
            }

            return None;
        }

        //
        // start loading
        //
        let fut = AssetLogger::<ImageAssetLoader>::load(resource.clone(), cx);

        let task = cx.background_executor().spawn(fut).shared();

        self.loading.insert(hash, task.clone());

        let entity = window.current_view();

        window
            .spawn(cx, {
                async move |cx| {
                    _ = task.await;

                    cx.on_next_frame(move |_, cx| {
                        cx.notify(entity);
                    });
                }
            })
            .detach();

        None
    }
}