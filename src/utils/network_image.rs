use std::{sync::Arc, time::Duration};

use gpui::{Asset, ImageCacheError, RenderImage};
use gpui_tokio::Tokio;
use anyhow::anyhow;

use crate::anilist::client::AniList;

#[derive(Clone, Hash)]
pub struct RemoteImage {
    pub url: String,
}

pub struct RemoteImageLoader;

impl Asset for RemoteImageLoader {
    type Source = RemoteImage;

    type Output = Result<Arc<RenderImage>, ImageCacheError>;

    fn load(
        source: Self::Source,
        cx: &mut gpui::App,
    ) -> impl Future<Output = Self::Output> + Send + 'static {
        let al_client = cx.global::<AniList>().clone();
        let task = Tokio::spawn_result(cx, async move {
                let response = al_client.client
                    .get(&source.url)
                    .timeout(Duration::from_secs(10))
                    .send()
                    .await?
                    .error_for_status()?;
        
                let bytes = response.bytes().await?;
        
                let dynamic = image::load_from_memory(&bytes)?;
        
                let mut rgba = dynamic.to_rgba8();
        
                // RGBA -> BGRA
                for pixel in rgba.pixels_mut() {
                    let [r, g, b, a] = pixel.0;
                    pixel.0 = [b, g, r, a];
                }
        
                let frame = image::Frame::new(rgba);
        
                Ok(Arc::new(RenderImage::new(vec![frame])))
            });
        async move {
                task.await.map_err(|e| {
                    ImageCacheError::Other(Arc::new(anyhow!(e)))
                })
            }
    }
}