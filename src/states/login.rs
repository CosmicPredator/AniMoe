use anyhow::anyhow;
use futures::{StreamExt, channel::mpsc};
use gpui::{
    App, AppContext, Bounds, Context, SharedString, TitlebarOptions, Window, WindowBounds,
    WindowOptions, px, size,
};
use gpui_component::Root;
use gpui_tokio::Tokio;
use log::{debug, error, info};
use tiny_http::{Response, Server};

use crate::{
    anilist::{client::AniList, token_callback::AccessTokenCallback},
    states::master::open_master_window,
    utils::constants::REDIRECT_URI,
    views::login::LoginView,
};

#[derive(Debug)]
pub struct AuthCodeCallback {
    pub code: String,
}

pub struct LoginState {
    auth_rx: Option<mpsc::UnboundedReceiver<AuthCodeCallback>>,
    pub button_is_loading: bool,
}

impl LoginState {
    pub fn new() -> Self {
        Self {
            auth_rx: None,
            button_is_loading: false,
        }
    }

    pub fn open_server(&mut self, cx: &mut Context<Self>, win: &mut Window) -> anyhow::Result<()> {
        let server = Server::http("127.0.0.1:2013").map_err(|err| anyhow!(err))?;

        let (tx, rx) = mpsc::unbounded();

        Tokio::spawn(cx, async move {
            for request in server.incoming_requests() {
                let url = format!("{}{}", REDIRECT_URI, request.url());

                if let Some(callback) = parse_auth_code(url) {
                    let _ = tx.unbounded_send(callback);

                    let _ = request.respond(Response::from_string(
                        "Authentication successful. You may close this window.",
                    ));
                    break;
                }

                let _ = request.respond(Response::from_string("Invalid callback"));
            }
        })
        .detach();

        self.auth_rx = Some(rx);
        self.handle_oauth_callback(cx, win);

        Ok(())
    }

    fn handle_oauth_callback(&mut self, cx: &mut Context<Self>, win: &mut Window) {
        let Some(mut rx) = self.auth_rx.take() else {
            return;
        };
        let al_client = cx.global::<AniList>().clone();
        let win_handle = win.window_handle();

        cx.spawn(async move |this, cx| {
            if let Some(callback) = rx.next().await {
                let fut = Tokio::spawn_result(cx, async move {
                    let access_token = al_client.get_access_token(callback.code).await?;
                    Ok(access_token)
                })
                .await;

                match fut {
                    Ok(access_token) => {
                        if let Some(this) = this.upgrade() {
                            this.update(cx, |this, cx| {
                                this.save_access_token(access_token)
                                    .expect("failed saving access token");
                                info!("saved access token");
                                this.open_master_window(cx);
                                let _ = win_handle.update(cx, |_, win, _| {
                                    win.remove_window();
                                });
                            })
                        }
                    }
                    Err(err) => {
                        error!("{err}")
                    }
                }
            }
        })
        .detach();
    }

    fn open_master_window(&self, cx: &mut Context<Self>) {
        open_master_window(cx);
    }

    fn save_access_token(&self, access_token_callback: AccessTokenCallback) -> anyhow::Result<()> {
        if let Some(config_dir) = dirs::config_local_dir() {
            let animoe_dir = config_dir.join("animoe");
            let config_file_path = animoe_dir.join("token.json");

            if !config_file_path.exists() {
                std::fs::create_dir_all(animoe_dir)?;
            }

            let json = serde_json::to_string(&access_token_callback)?;
            Ok(std::fs::write(config_file_path, json)?)
        } else {
            let err = anyhow!("failed to save token file");
            error!("{err}");
            Err(err)
        }
    }
}

fn parse_auth_code(url: String) -> Option<AuthCodeCallback> {
    println!("{}", url);
    let url_parsed = url::Url::parse(url.as_str()).unwrap();
    let code = url_parsed
        .query_pairs()
        .find(|(key, _)| key == "code")
        .map(|(_, value)| value.into_owned());

    code.map(|code| AuthCodeCallback { code })
}

pub fn open_login_window(cx: &mut App) {
    let bounds = Bounds::centered(None, size(px(500.), px(500.0)), cx);
    let window_options = WindowOptions {
        window_bounds: Some(WindowBounds::Windowed(bounds)),
        titlebar: Some(TitlebarOptions {
            title: Some(SharedString::new("AniMoe")),
            ..Default::default()
        }),
        ..Default::default()
    };

    let _ = cx.open_window(window_options, |window, cx| {
        debug!("opening login view window");
        let login_view = cx.new(|cx| LoginView::new(cx, window));
        cx.new(|cx| Root::new(login_view, window, cx))
    });
}
