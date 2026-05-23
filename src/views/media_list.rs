use std::ops::Range;

use gpui::{
    App, AppContext, Context, Entity, InteractiveElement, IntoElement, ParentElement, Render, Styled, StyledImage, TextOverflow, UniformListScrollHandle, Window, div, img, px, uniform_list
};
use gpui_component::{
    Icon, StyledExt, button::{Button, ButtonVariants}, h_flex, input::{Input, InputState}, progress::Progress, scroll::ScrollableElement, select::{Select, SelectState}, spinner::Spinner
};

use crate::{anilist::media_list::{Entry, List}, states::master::MasterState, utils::{image_cache::simple_lru_cache, network_image::{RemoteImage, RemoteImageLoader}}};

const CARD_WIDTH: f32 = 130.;
const CARD_GAP: f32 = 15.0;

pub struct MediaListPage {
    scroll: UniformListScrollHandle,
    state: Entity<MasterState>
}

impl MediaListPage {
    pub fn new(_cx: &mut Context<Self>, _window: &mut Window, entity: Entity<MasterState>) -> Self {
        Self {
            scroll: UniformListScrollHandle::new(),
            state: entity
        }
    }
}

impl Render for MediaListPage {
    fn render(
        &mut self,
        window: &mut gpui::Window,
        cx: &mut gpui::prelude::Context<Self>,
    ) -> impl gpui::prelude::IntoElement {
        div()
            .v_flex()
            .size_full()
            .ml(px(40.))
            .mr(px(40.))
            .mt(px(40.))
            .gap(px(20.))
            .child(div().text_3xl().font_semibold().child("Anime List"))
            .child(self.tool_bar(cx, window))
            .child(self.media_list(cx, window))
    }
}

impl MediaListPage {
    // top tool bar which contains search and filter buttons
    fn tool_bar(&self, cx: &mut Context<Self>, window: &mut Window) -> impl IntoElement {
        let input_state = cx.new(|cx| InputState::new(window, cx).placeholder("Search anime..."));
        let select_state = cx.new(|cx| SelectState::new(vec!["hello", "world"], None, window, cx));

        div()
            .w_full()
            .h_flex()
            .justify_between()
            .child(
                div()
                    .h_flex()
                    .gap_2()
                    .child(Input::new(&input_state).w(px(250.0)).cleanable(true))
                    .child(
                        Button::new("search-btn")
                            .icon(Icon::empty().path("./assets/search.svg"))
                            .tooltip("Search"),
                    ),
            )
            .child(
                div()
                    .h_flex()
                    .gap_2()
                    .child(
                        Select::new(&select_state)
                            .w(px(200.0))
                            .placeholder("Select a status"),
                    )
                    .child(
                        Button::new("filter-btn")
                            .icon(Icon::empty().path("./assets/list-filter.svg"))
                            .tooltip("Filter"),
                    ),
            )
    }

    // actual media uniform list
    fn media_list(&self, cx: &mut Context<Self>, window: &mut Window) -> impl IntoElement {
        let avail_width = window.bounds().size.width.as_f32() - 80. - 255.;
        let columns = ((avail_width + CARD_GAP) / (CARD_WIDTH + CARD_GAP))
            .floor()
            .max(1.0) as usize;

        let anime_list = self.state.read(cx).anime_list.clone();
        if anime_list.is_none() {
            return div()
                .size_full()
                .v_flex()
                .justify_center()
                .items_center()
                .child(
                    Spinner::new()
                        .icon(Icon::empty().path("./assets/spinner.svg"))
                );
        }

        let entries = anime_list.unwrap()[0].entries.clone();
        let row_count = entries.len().div_ceil(columns);

        div()
            .size_full()
            .image_cache(simple_lru_cache("media-list-cache", 500))
            .vertical_scrollbar(&self.scroll)
            .child(uniform_list(
            "media-list",
            row_count,
            cx.processor(move |_this, range: Range<usize>, _, _cx| {
                range
                    .map(|ix| {
                        let start = ix * columns;
                        let end = (start + columns).min(entries.len());

                        h_flex()
                            .id(format!("media-card-{}", ix))
                            .gap(px(CARD_GAP))
                            .pb(px(CARD_GAP))
                            .w_full()
                            .justify_center()
                            .children(entries[start..end].iter().map(|anime| {
                                if anime.media.next_airing_episode.is_none() {
                                    media_card(
                                        anime.media_id as usize,
                                        anime.media.title.user_preferred.clone(),
                                        anime.media.format.clone().unwrap(),
                                        anime.media.episodes.unwrap(),
                                        anime.progress,
                                        0,
                                        anime.media.cover_image.large.clone()
                                    )
                                } else {
                                    media_card(
                                        anime.media_id as usize,
                                        anime.media.title.user_preferred.clone(),
                                        anime.media.format.clone().unwrap(),
                                        anime.media.episodes.unwrap(),
                                        anime.progress,
                                        anime.media.next_airing_episode.clone().unwrap().episode,
                                        anime.media.cover_image.large.clone()
                                    )
                                }
                            }))
                    })
                    .collect::<Vec<_>>()
            }),
        )
        .h_full()
        .track_scroll(&self.scroll))
    }
}

// media list card component
fn media_card(
    ix: usize,
    title: String,
    format: String,
    episodes: i64,
    progress: i64,
    next_airing_episode: i64,
    cover_image_url: String
) -> impl IntoElement {
    let image = RemoteImage{
        url: cover_image_url
    };
    
    div()
        .w(px(CARD_WIDTH))
        .h(px(270.))
        .rounded_sm()
        .bg(gpui::opaque_grey(0.3, 0.3))
        .p(px(5.0))
        .child(
            div()
                .v_flex()
                .size_full()
                .child(
                    div().w_full().h(px(190.)).rounded_md().overflow_hidden().child(
                        img(move |win: &mut Window, cx: &mut App| {
                            win.use_asset::<RemoteImageLoader>(&image, cx)
                        })
                        .size_full()
                        .rounded_sm()
                        .object_fit(gpui::ObjectFit::Cover),
                    ),
                )
                .child(
                    div()
                        .text_sm()
                        .mt(px(4.0))
                        .text_overflow(TextOverflow::Truncate("...".into()))
                        .child(title),
                )
                .child(div().text_size(px(10.0)).child(format!(
                    "{} • {} Ep Behind",
                    format,
                    (next_airing_episode - progress).max(0)
                )))
                .child(div().h(px(10.0)))
                .child(
                    div()
                        .h_flex()
                        .justify_between()
                        .child(
                            Button::new(format!("add-btn-{}", ix))
                                .compact()
                                .rounded_sm()
                                .h(px(20.0))
                                .text_xs()
                                .primary()
                                .child(
                                    div()
                                        .text_xs()
                                        .child(format!("{} / {} +", progress, episodes)),
                                ),
                        )
                        .child(
                            div()
                                .h(px(15.0))
                                .w(px(15.0))
                                .mr(px(5.0))
                                .bg(gpui::green())
                                .rounded_full(),
                        ),
                )
                .child(div().h(px(5.0))),
        )
}
