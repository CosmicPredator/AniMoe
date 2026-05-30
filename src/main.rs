#![windows_subsystem = "windows"]

use std::path::PathBuf;

use gpui::{App, SharedString};
use gpui_component::{Theme, ThemeRegistry};
use log::{debug, info};

use crate::{
    anilist::client::AniList,
    assets::Assets,
    states::{login::open_login_window, master::open_master_window},
};

mod anilist;
mod assets;
mod states;
mod utils;
mod views;

pub fn init_theme(cx: &mut App) {
    let theme_name = SharedString::from("macOS Classic Dark");
    // Load and watch themes from ./themes directory
    if ThemeRegistry::watch_dir(PathBuf::from("./themes"), cx, move |cx| {
        if let Some(theme) = ThemeRegistry::global(cx).themes().get(&theme_name).cloned() {
            Theme::global_mut(cx).apply_config(&theme);
        }
    })
    .is_err()
    {}
}

fn is_token_exists() -> bool {
    if let Some(config_dir) = dirs::config_local_dir() {
        let animoe_dir = config_dir.join("animoe");
        let config_file_path = animoe_dir.join("token.json");
        return config_file_path.exists();
    }
    false
}

fn run_app() {
    gpui_platform::application()
        .with_assets(Assets)
        .run(move |cx| {
            debug!("initializing tokio, gpui_component and theme");
            gpui_tokio::init(cx);
            gpui_component::init(cx);
            init_theme(cx);

            let al_client = AniList::new().unwrap();
            cx.set_global(al_client);

            if is_token_exists() {
                open_master_window(cx);
            } else {
                open_login_window(cx);
            }

            cx.activate(true);
        });
}

fn main() {
    dotenvy::dotenv().ok();
    env_logger::init();
    info!("loaded env file");

    info!("starting application");
    run_app();
    info!("stopping application");
}
