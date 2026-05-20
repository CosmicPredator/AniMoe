use gpui::{
    AppContext, Context, Entity, ParentElement, Render, SharedString, Styled, div, img, px,
};
use gpui_component::{
    Icon, StyledExt,
    button::{Button, ButtonVariants},
};

use crate::states::login::LoginState;

pub struct LoginView {
    state: Entity<LoginState>,
}

impl LoginView {
    pub fn new(cx: &mut Context<Self>) -> Self {
        let state = cx.new(|_| LoginState::new());
        Self { state }
    }
}

impl Render for LoginView {
    fn render(
        &mut self,
        _window: &mut gpui::Window,
        cx: &mut gpui::prelude::Context<Self>,
    ) -> impl gpui::prelude::IntoElement {
        let state = self.state.read(cx);
        div()
            .v_flex()
            .size_full()
            .justify_center()
            .items_center()
            .child(
                div()
                    .v_flex()
                    .items_center()
                    .gap_5()
                    .child(img("./assets/animoe.png").w(px(120.)).h(px(120.)))
                    .child(
                        Button::new("login-btn")
                            .label("Login with AniList")
                            .loading_icon(
                                Icon::empty().path(SharedString::new("./assets/spinner.svg")),
                            )
                            .loading(state.btn_clicked)
                            .primary()
                            .on_click(cx.listener(|this, _, _, cx| {
                                this.state.update(cx, |this, cx| {
                                    this.toggle_btn_clicked(cx);
                                    if this.btn_clicked {
                                        this.open_login_url(cx);
                                    }
                                });
                            })),
                    ),
            )
    }
}
