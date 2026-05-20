use gpui::Context;

pub struct LoginState {
    pub btn_clicked: bool,
}

impl LoginState {
    pub fn new() -> Self {
        Self { btn_clicked: false }
    }

    pub fn toggle_btn_clicked(&mut self, cx: &mut Context<Self>) -> () {
        self.btn_clicked = true;
        cx.notify();
    }

    pub fn open_login_url(&self, cx: &mut Context<Self>) -> () {
        cx.open_url(crate::utils::constants::AL_AUTH_URL);
    }
}
