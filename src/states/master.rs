use gpui::{App, AppContext, Bounds, Context, SharedString, TitlebarOptions, WindowBounds, WindowOptions, px, size};
use gpui_component::Root;
use gpui_tokio::Tokio;
use log::{debug, error};

use crate::{
    anilist::{client::AniList, viewer::Viewer},
    utils::enums::MediaType, views::master::MasterView,
};

#[derive(PartialEq, Eq, Clone, Copy)]
pub enum Page {
    Home,
    Anime,
    Manga,
    Explore,
    Notifications,
    User,
    Settings,
}

pub struct MasterState {
    pub current_page: Page,
    pub viewer: Option<Viewer>,
    pub anime_list: Option<Vec<crate::anilist::media_list::List>>,
}

impl MasterState {
    pub fn new() -> Self {
        Self {
            current_page: Page::Home,
            viewer: None,
            anime_list: None,
        }
    }

    pub fn change_page(&mut self, cx: &mut Context<Self>, item: Page) {
        self.current_page = item;
        cx.notify();
    }

    pub fn fetch_viewer(&mut self, cx: &mut Context<Self>) {
        let mut al_client = cx.global::<AniList>().clone();

        let fut = Tokio::spawn_result(cx, async move {
            let data = al_client.fetch_viewer().await?;
            Ok(data.data.viewer)
        });

        cx.spawn(async move |this, cx| {
            let result = fut.await;

            match result {
                Ok(result) => {
                    if let Some(this) = this.upgrade() {
                        this.update(cx, |this, cx| {
                            this.viewer = Some(result);
                            this.fetch_anime_list(cx);
                            println!("{:?}", this.anime_list);
                            cx.notify();
                        });
                    }
                }
                Err(err) => {
                    error!("failed to fetch viewer: {}", err);
                }
            }
        })
        .detach();
    }

    pub fn fetch_anime_list(&mut self, cx: &mut Context<Self>) {
        let mut al_client = cx.global::<AniList>().clone();
        if let Some(ref viewer) = self.viewer {
            let user_id = viewer.id;
            let fut = Tokio::spawn_result(cx, async move {
                let data = al_client
                    .fetch_anime_list(user_id, MediaType::ANIME)
                    .await?;
                Ok(data.data.media_list_collection.lists)
            });

            cx.spawn(async move |this, cx| {
                let result = fut.await;

                match result {
                    Ok(result) => {
                        if let Some(this) = this.upgrade() {
                            this.update(cx, |this, cx| {
                                this.anime_list = Some(result);
                                cx.notify();
                            });
                        }
                    }
                    Err(err) => {
                        error!("failed to fetch viewer: {}", err);
                    }
                }
            })
            .detach();
        }
    }
}

pub fn open_master_window(cx: &mut App) {
    let bounds = Bounds::centered(None, size(px(1280.), px(720.0)), cx);
    let window_options = WindowOptions {
        window_bounds: Some(WindowBounds::Windowed(bounds)),
        titlebar: Some(TitlebarOptions {
            title: Some(SharedString::new("AniMoe")),
            ..Default::default()
        }),
        ..Default::default()
    };

    debug!("opening master window");
    cx.open_window(window_options, |window, cx| {
        //let root = cx.new(|cx| LoginView::new(cx));
        debug!("creating master view");
        let root = cx.new(|cx| MasterView::new(cx, window));
        cx.new(|cx| Root::new(root, window, cx))
    })
    .expect("Failed to open master window");
}