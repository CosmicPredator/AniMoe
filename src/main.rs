use std::path::PathBuf;

use gpui::{
    App, AppContext, Bounds, SharedString, TitlebarOptions, WindowBounds,
    WindowOptions, px, size,
};
use gpui_component::{Root, Theme, ThemeRegistry};

use crate::{
    assets::Assets, views::master::MasterView
};

mod assets;
mod states;
mod utils;
mod views;
mod anilist;

pub fn init_theme(cx: &mut App) {
    let theme_name = SharedString::from("macOS Classic Dark");
    // Load and watch themes from ./themes directory
    if ThemeRegistry::watch_dir(PathBuf::from("./themes"), cx, move |cx| {
        if let Some(theme) = ThemeRegistry::global(cx).themes().get(&theme_name).cloned() {
            Theme::global_mut(cx).apply_config(&theme);
        }
    }).is_err() {}
}

fn main() {
    gpui_platform::application().with_assets(Assets).run(move |cx| {
        let bounds = Bounds::centered(None, size(px(1280.), px(720.0)), cx);
        let window_options = WindowOptions {
            window_bounds: Some(WindowBounds::Windowed(bounds)),
            titlebar: Some(TitlebarOptions {
                title: Some(SharedString::new("AniMoe")),
                ..Default::default()
            }),
            ..Default::default()
        };

        gpui_component::init(cx);
        init_theme(cx);

        cx.open_window(window_options, |window, cx| {
            //let root = cx.new(|cx| LoginView::new(cx));
            let root = cx.new(|cx| MasterView::new(cx, window));
            cx.new(|cx| Root::new(root, window, cx))
        })
        .expect("Failed to open window");
        cx.activate(true);
    });
}

// fn main() {
//     println!("{}", gpui::guess_compositor())
// }