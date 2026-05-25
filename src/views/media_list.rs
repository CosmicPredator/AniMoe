use std::ops::Range;

use gpui::{
    App, AppContext, Context, Entity, InteractiveElement, IntoElement, ParentElement,
    Render, SharedString, Styled, StyledImage, TextOverflow,
    UniformListScrollHandle, Window, div, img, prelude::FluentBuilder, px, uniform_list,
};
use gpui_component::{
    Disableable, Icon, StyledExt,
    button::{Button, ButtonVariants},
    h_flex,
    input::Input,
    scroll::ScrollableElement,
    select::Select,
    spinner::Spinner,
};
use log::info;

use crate::{
    anilist::media_list::Entry,
    states::{master::MasterState, media_list::MediaListState},
    utils::{
        image_cache::simple_lru_cache,
        network_image::{RemoteImage, RemoteImageLoader},
    },
};

const CARD_WIDTH: f32 = 130.;
const CARD_GAP: f32 = 15.0;

pub struct MediaListPage {
    scroll: UniformListScrollHandle,
    state: Entity<MediaListState>,
}

impl MediaListPage {
    pub fn new(cx: &mut Context<Self>, window: &mut Window, entity: Entity<MasterState>) -> Self {
        let ml_state = cx.new(|cx| {
            MediaListState::new(cx, window, entity, crate::utils::enums::MediaType::ANIME)
        });

        Self {
            scroll: UniformListScrollHandle::new(),
            state: ml_state,
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
        let state = self.state.read(cx);

        div()
            .w_full()
            .h_flex()
            .justify_between()
            .child(
                div()
                    .h_flex()
                    .gap_2()
                    .child(
                        Input::new(&state.search_input_state)
                            .w(px(250.0))
                            .cleanable(true),
                    )
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
                        Select::new(&state.status_select_state)
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

        let anime_list = self.state.read(cx).selected_list.clone();
        let current_status = self.state.read(cx).current_status.clone();

        if anime_list.is_none() {
            return div()
                .size_full()
                .v_flex()
                .justify_center()
                .items_center()
                .child(Spinner::new().icon(Icon::empty().path("./assets/spinner.svg")));
        }

        let entries = anime_list.unwrap().clone();
        let row_count = entries.len().div_ceil(columns);

        div()
            .size_full()
            .image_cache(simple_lru_cache("media-list-cache", 500))
            .vertical_scrollbar(&self.scroll)
            .child(
                uniform_list(
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
                                        media_card(
                                            anime.media_id as usize,
                                            anime.clone(),
                                            current_status.clone(),
                                            anime.media.cover_image.large.clone(),
                                        )
                                    }))
                            })
                            .collect::<Vec<_>>()
                    }),
                )
                .h_full()
                .track_scroll(&self.scroll),
            )
    }
}

// media list card component
fn media_card(
    ix: usize,
    entry: Entry,
    status: SharedString,
    cover_image_url: SharedString,
) -> impl IntoElement {
    let image = RemoteImage {
        url: cover_image_url.to_string(),
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
                    div()
                        .w_full()
                        .h(px(190.))
                        .rounded_md()
                        .overflow_hidden()
                        .child(
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
                        .child(entry.media.title.user_preferred),
                )
                .when_else(
                    status == "Watching",
                    |this| {
                        let next_airing = entry
                            .media
                            .next_airing_episode
                            .map(|f| f.episode)
                            .unwrap_or_default();
                        let text = if next_airing != 0 {
                            let ep_behind = (next_airing - 1) - entry.progress;

                            if ep_behind > 0 {
                                format!(
                                    "{} • {} Ep Behind",
                                    entry.media.format.clone().unwrap_or("?".into()),
                                    ep_behind
                                )
                            } else {
                                entry.media.format.clone().unwrap_or("?".into()).to_string()
                            }
                        } else {
                            entry.media.format.clone().unwrap_or("?".into()).to_string()
                        };

                        this.child(div().text_size(px(10.0)).child(text))
                    },
                    |this| {
                        this.child(
                            div().text_size(px(10.0)).child(
                                entry.media.format.clone().unwrap_or("?".into()).to_string(),
                            ),
                        )
                    },
                )
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
                                .disabled(status == "Completed")
                                .child(div().text_xs().child(format!(
                                    "{} / {} +",
                                    entry.progress,
                                    entry.media.episodes.unwrap_or(0)
                                ))),
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
