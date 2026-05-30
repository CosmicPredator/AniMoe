use gpui::{
    AppContext, Context, Entity, ParentElement, Render, SharedString, Styled, Window, div, img, px
};
use gpui_component::{
    Icon, StyledExt, button::{Button, ButtonVariants}
};

use crate::{states::login::LoginState, utils::constants::AL_AUTH_URL};

pub struct LoginView {
    state: Entity<LoginState>,
}

impl LoginView {
    pub fn new(cx: &mut Context<Self>, win: &mut Window) -> Self {
        let state = cx.new(|_| LoginState::new());
        state.update(cx, |this, cx| {
            let _ = this.open_server(cx, win);
        });
        
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
                        div()
                            .text_2xl()
                            .font_semibold()
                            .child("AniMoe for AniList")
                    )
                    .child(
                        Button::new("login-btn")
                            .label("Login with AniList")
                            .loading(state.button_is_loading)
                            .loading_icon(
                                Icon::empty().path(SharedString::new("./assets/spinner.svg")),
                            )
                            .primary()
                            .on_click(cx.listener(|this, _, _, cx| {
                                cx.open_url(AL_AUTH_URL);
                                this.state.update(cx, |this, cx| {
                                    this.button_is_loading = true;
                                    cx.notify();
                                })
                            }))
                    ),
            )
    }
}
