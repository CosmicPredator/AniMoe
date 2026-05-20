use gpui::Context;

#[derive(PartialEq, Eq, Clone, Copy)]
pub enum Page {
    Home,
    Anime,
    Manga,
    Explore,
    Notifications,
    User,
    Settings,
}

pub struct HomeState {
    pub current_page: Page,
}

impl HomeState {
    pub fn new() -> Self {
        Self {
            current_page: Page::Home,
        }
    }

    pub fn change_page(&mut self, cx: &mut Context<Self>, item: Page) -> () {
        self.current_page = item;
        cx.notify();
    }
}
