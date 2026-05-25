use gpui::Context;
use gpui_tokio::Tokio;
use log::{debug, error, info};

use crate::{anilist::{client::AniList, viewer::Viewer}, utils::enums::MediaType};

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
    pub anime_list: Option<Vec<crate::anilist::media_list::List>>
}

impl MasterState {
    pub fn new() -> Self {
        Self {
            current_page: Page::Home,
            viewer: None,
            anime_list: None
        }
    }

    pub fn change_page(&mut self, cx: &mut Context<Self>, item: Page) {
        self.current_page = item;
        cx.notify();
    }

    pub fn fetch_viewer(&mut self, cx: &mut Context<Self>) {
        let al_client = cx.global::<AniList>().clone();
        
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
                },
                Err(err) => {
                    error!("failed to fetch viewer: {}", err);
                }
            }
        }).detach();
    }

    pub fn fetch_anime_list(&mut self, cx: &mut Context<Self>) {
        let al_client = cx.global::<AniList>().clone();
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
                    },
                    Err(err) => {
                        error!("failed to fetch viewer: {}", err);
                    }
                }
            }).detach();
        }
    }
}
