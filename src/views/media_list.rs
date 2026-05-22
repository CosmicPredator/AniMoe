use std::ops::Range;

use gpui::{
    AppContext, Context, InteractiveElement, IntoElement, ParentElement, Render, Styled,
    StyledImage, TextOverflow, UniformListScrollHandle, Window, div, img, px, uniform_list,
};
use gpui_component::{
    Icon, StyledExt,
    button::{Button, ButtonVariants},
    h_flex,
    input::{Input, InputState},
    select::{Select, SelectState},
};

const CARD_WIDTH: f32 = 120.;
const CARD_GAP: f32 = 10.0;

pub struct MediaListPage {
    scroll: UniformListScrollHandle,
    anime: Vec<i32>,
}

impl MediaListPage {
    pub fn new(_cx: &mut Context<Self>, _window: &mut Window) -> Self {
        Self {
            scroll: UniformListScrollHandle::new(),
            anime: (0..100).collect(),
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

        let row_count = self.anime.len().div_ceil(columns);
        
        uniform_list(
            "media-list",
            row_count,
            cx.processor(move |this, range: Range<usize>, _, _cx| {
                range
                    .map(|ix| {
                        let start = ix * columns;
                        let end = (start + columns).min(this.anime.len());
    
                        h_flex()
                            .id(format!("media-card-{}", ix))
                            .gap(px(CARD_GAP))
                            .pb(px(CARD_GAP))
                            .w_full()
                            .justify_center()
                            .children(
                                this.anime[start..end]
                                    .iter()
                                    .map(|id| media_card(*id as usize)),
                            )
                    })
                    .collect::<Vec<_>>()
            }),
        )
        .h_full()
        .track_scroll(&self.scroll)
    }
}

// media list card component
fn media_card(ix: usize) -> impl IntoElement {
    div()
        .w(px(120.))
        .rounded_sm()
        .bg(gpui::opaque_grey(0.3, 0.3))
        .p(px(5.0))
        .child(
            div()
                .v_flex()
                .size_full()
                .child(
                    div().rounded_sm().h(px(170.)).child(
                        img("./assets/cover.jpg")
                            .size_full()
                            .rounded_sm()
                            .object_fit(gpui::ObjectFit::Contain),
                    ),
                )
                .child(
                    div()
                        .text_sm()
                        .text_overflow(TextOverflow::Truncate("...".into()))
                        .child("The Ramparts of Ice"),
                )
                .child(div().text_size(px(10.0)).child("Tv Show • 1 Ep Behind"))
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
                                .child(div().text_xs().child("2 / 12 +")),
                        )
                        .child(
                            div()
                                .h(px(15.0))
                                .w(px(15.0))
                                .bg(gpui::green())
                                .rounded_full(),
                        ),
                )
                .child(div().h(px(5.0))),
        )
}