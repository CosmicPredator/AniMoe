use gpui::{
    AppContext, Context, Entity, ParentElement, Render, Styled, Window, div
};
use gpui_component::StyledExt;

use crate::{states::home::HomeState, views::media_list::MediaListPage};

pub struct MasterView {
    pub state: Entity<HomeState>,
    pub ml_page: Entity<MediaListPage>
}

impl MasterView {
    pub fn new(cx: &mut Context<Self>, window: &mut Window) -> Self {
        Self {
            state: cx.new(|_| HomeState::new()),
            ml_page: cx.new(|cx| MediaListPage::new(cx, window))
        }
    }
}

impl Render for MasterView {
    fn render(
        &mut self,
        window: &mut gpui::Window,
        cx: &mut gpui::prelude::Context<Self>,
    ) -> impl gpui::prelude::IntoElement {
        let current_nav = self.state.read(cx).current_page;
        div()
            .h_flex()
            .size_full()
            .child(self.navbar(current_nav, cx))
            .child(self.ml_page.clone())
    }
}
