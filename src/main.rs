#![windows_subsystem = "windows"]


use gpui::App;
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

const THEME_NAME: &str = "macOS Classic Dark";

pub fn init_theme(cx: &mut App) {
    if let Some(theme_file) = Assets::get("macos.json")
        && let Ok(json) = std::str::from_utf8(theme_file.data.as_ref())
    {
        let registry = ThemeRegistry::global_mut(cx);
        let _ = registry.load_themes_from_str(json);
    
        if let Some(theme) = registry.themes().get(THEME_NAME).cloned() {
            Theme::global_mut(cx).apply_config(&theme);
        }
    }
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
