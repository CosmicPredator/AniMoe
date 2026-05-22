
use gpui::{
    AppContext, Context, IntoElement, ParentElement, Render, Styled,
    StyledImage, TextOverflow, Window, div, img, px,
};
use gpui_component::{
    Icon, StyledExt,
    button::{Button, ButtonVariants},
    input::{Input, InputState},
    scroll::ScrollableElement,
    select::{Select, SelectState}, v_virtual_list,
};


pub struct MediaListPage {}

impl MediaListPage {
    pub fn new(_cx: &mut Context<Self>, _window: &mut Window) -> Self {
        Self { }
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
            .child(
                div()
                    .h_flex()
                    .flex_wrap()
                    .justify_center()
                    .overflow_y_scrollbar()
                    .gap_2()
                    .children((0..50).map(|ix| self.media_card(ix))),
            )
    }
}

impl MediaListPage {
    fn tool_bar(&self, cx: &mut Context<Self>, window: &mut Window) -> impl IntoElement {
        let input_state = cx.new(|cx| InputState::new(window, cx).placeholder("Search anime..."));
        let select_state = cx.new(|cx| SelectState::new(
            vec!["hello", "world"],
            None,
            window,
            cx
        ));
        
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

    fn media_card(&self, ix: usize) -> impl IntoElement {
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
                    ),
            )
    }
}
