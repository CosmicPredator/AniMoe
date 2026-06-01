use gpui::{Context, IntoElement, ParentElement, Styled, div};
use gpui_component::{
    Icon, Side,
    sidebar::{Sidebar, SidebarGroup, SidebarMenu, SidebarMenuItem},
};

use crate::{states::master::Page, views::master::MasterView};

const VERSION: &str = env!("CARGO_PKG_VERSION");

impl MasterView {
    pub fn navbar(&self, current_page: Page, cx: &mut Context<Self>) -> impl IntoElement {
        let viewer = self.state.read(cx);
        Sidebar::new("sidebar-1")
            .side(Side::Left)
            .collapsible(true)
            .child(
                SidebarGroup::new("Navigation").child(
                    SidebarMenu::new()
                        .child(
                            SidebarMenuItem::new("Home")
                                .active(current_page == Page::Home)
                                .icon(Icon::empty().path("house.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Home);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Anime")
                                .icon(Icon::empty().path("film.svg"))
                                .active(current_page == Page::Anime)
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Anime);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Manga")
                                .active(current_page == Page::Manga)
                                .icon(Icon::empty().path("book-copy.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Manga);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Explore")
                                .active(current_page == Page::Explore)
                                .icon(Icon::empty().path("blocks.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Explore);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Notifications")
                                .active(current_page == Page::Notifications)
                                .icon(Icon::empty().path("bell.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Notifications);
                                    })
                                })),
                        )
                        .child(if let Some(ref viewer) = viewer.viewer {
                            SidebarMenuItem::new(viewer.name.clone())
                                .active(current_page == Page::User)
                                .icon(Icon::empty().path("user.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::User);
                                    })
                                }))
                        } else {
                            SidebarMenuItem::new("Loading...")
                                .active(current_page == Page::User)
                                .icon(Icon::empty().path("user.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::User);
                                    })
                                }))
                        })
                        .child(
                            SidebarMenuItem::new("Settings")
                                .active(current_page == Page::Settings)
                                .icon(Icon::empty().path("bolt.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Settings);
                                    })
                                })),
                        ),
                ),
            )
            .footer(
                div()
                    .text_sm()
                    .text_color(gpui::opaque_grey(1.0, 0.3))
                    .child(format!("AniMoe v{}", VERSION)),
            )
    }
}
