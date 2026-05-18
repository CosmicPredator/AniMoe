use std::path::PathBuf;

use gpui::{App, AppContext, Application, Bounds, Context, Entity, ParentElement, Render, SharedString, Styled, TitlebarOptions, WindowBounds, WindowOptions, div, img, px, size};
use gpui_component::{Icon, Root, StyledExt, Theme, ThemeRegistry, button::{Button, ButtonVariants}};

use crate::assets::Assets;

mod assets;

pub struct HomeView {
    is_loading: Entity<bool>,
}

impl HomeView {
    pub fn new(cx: &mut Context<Self>) -> Self {
        Self { is_loading: cx.new(|_| false) }
    }
}

impl Render for HomeView {
    fn render(&mut self, _window: &mut gpui::Window, cx: &mut gpui::prelude::Context<Self>) -> impl gpui::prelude::IntoElement {
        let is_loading = self.is_loading.read(cx).clone();
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
                            .loading_icon(Icon::empty().path(SharedString::new("./assets/spinner.svg")))
                            .loading(is_loading)
                            .primary()
                            .on_click(cx.listener(|this, _, _, cx| {
                                this.is_loading.update(cx, |this, cx| {
                                    *this = true;
                                    cx.notify();
                                });
                                cx.open_url("https://anilist.co/api/v2/oauth/authorize?client_id=13389&response_type=token");
                            }))
                    )   
            )
    }
}

pub fn init_theme(cx: &mut App) {
    let theme_name = SharedString::from("macOS Classic Dark");
    // Load and watch themes from ./themes directory
    if let Err(_) = ThemeRegistry::watch_dir(PathBuf::from("./themes"), cx, move |cx| {
        if let Some(theme) = ThemeRegistry::global(cx)
            .themes()
            .get(&theme_name)
            .cloned()
        {
            Theme::global_mut(cx).apply_config(&theme);
        }
    }) {}
}

fn main() {
    Application::new()
        .with_assets(Assets)
        .run(move |cx| {
        let bounds = Bounds::centered(None, size(px(1280.), px(720.0)), cx);
        let window_options = WindowOptions {
            window_bounds: Some(WindowBounds::Windowed(bounds)),
            titlebar: Some(TitlebarOptions{
                title: Some(SharedString::new("AniMoe")),
                ..Default::default()
            }),
            ..Default::default()
        };
        
        gpui_component::init(cx);
        init_theme(cx);
        
        cx.open_window(window_options, |window, cx| {
            let root = cx.new(|cx| HomeView::new(cx));
            cx.new(|cx| Root::new(root, window, cx))
        }).expect("Failed to open window");
        cx.activate(true);
    });
}