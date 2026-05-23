use gpui::Context;
use gpui_tokio::Tokio;
use log::error;

use crate::anilist::{client::AniList, viewer::Viewer};

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
}

impl MasterState {
    pub fn new() -> Self {
        Self {
            current_page: Page::Home,
            viewer: None
        }
    }

    pub fn change_page(&mut self, cx: &mut Context<Self>, item: Page) {
        self.current_page = item;
        cx.notify();
    }

    pub fn fetch_viewer(&mut self, cx: &mut Context<Self>) {
        let al_client = cx.global::<AniList>().clone();
        
        let fut = Tokio::spawn_result(cx, async move {
            let data = al_client.get_viewer().await?;
            Ok(data.data.viewer)
        });

        cx.spawn(async move |this, cx| {
            let result = fut.await;

            match result {
                Ok(result) => {
                    if let Some(this) = this.upgrade() {
                        this.update(cx, |this, cx| {
                            this.viewer = Some(result);
                            cx.notify();
                        });
                    }
                },
                Err(err) => {
                    error!("failed to fetch viewer: {}", err);
                }
            }
        }).detach();
    }
}
