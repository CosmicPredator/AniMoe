use gpui::{AppContext, Context, Entity, ParentElement, Render, Styled, Window, div};
use gpui_component::StyledExt;

use crate::{states::master::MasterState, views::media_list::MediaListPage};

pub struct MasterView {
    pub state: Entity<MasterState>,
    pub ml_page: Entity<MediaListPage>,
}

impl MasterView {
    pub fn new(cx: &mut Context<Self>, window: &mut Window) -> Self {
        let master_state = cx.new(|_| MasterState::new());
        master_state.update(cx, |this, cx| {
            this.fetch_viewer(cx);
        });

        let entry_list = master_state.read(cx).anime_list.clone();
        let ml_page = cx.new(|cx| {
            MediaListPage::new(
                cx,
                window,
                master_state.clone()
            )
        });

        Self {
            state: master_state,
            ml_page
        }
    }
}

impl Render for MasterView {
    fn render(
        &mut self,
        _window: &mut gpui::Window,
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
