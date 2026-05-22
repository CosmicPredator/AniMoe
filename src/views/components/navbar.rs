use gpui::{Context, IntoElement, ParentElement, Styled, div};
use gpui_component::{
    Icon, Side, h_flex,
    sidebar::{Sidebar, SidebarFooter, SidebarGroup, SidebarMenu, SidebarMenuItem},
    v_flex,
};

use crate::{states::home::Page, views::master::MasterView};

impl MasterView {
    pub fn navbar(&self, current_page: Page, cx: &mut Context<Self>) -> impl IntoElement {
        Sidebar::new("sidebar-1")
            .side(Side::Left)
            .collapsible(true)
            .child(
                SidebarGroup::new("Navigation").child(
                    SidebarMenu::new()
                        .child(
                            SidebarMenuItem::new("Home")
                                .active(current_page == Page::Home)
                                .icon(Icon::empty().path("./assets/house.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Home);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Anime")
                                .icon(Icon::empty().path("./assets/film.svg"))
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
                                .icon(Icon::empty().path("./assets/book-copy.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Manga);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Explore")
                                .active(current_page == Page::Explore)
                                .icon(Icon::empty().path("./assets/blocks.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Explore);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Notifications")
                                .active(current_page == Page::Notifications)
                                .icon(Icon::empty().path("./assets/bell.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Notifications);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Cosmic Predator")
                                .active(current_page == Page::User)
                                .icon(Icon::empty().path("./assets/user.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Explore);
                                    })
                                })),
                        )
                        .child(
                            SidebarMenuItem::new("Settings")
                                .active(current_page == Page::Settings)
                                .icon(Icon::empty().path("./assets/bolt.svg"))
                                .on_click(cx.listener(|this, _, _, cx| {
                                    this.state.update(cx, |this, cx| {
                                        this.change_page(cx, Page::Notifications);
                                    })
                                })),
                        )
                ),
            )
            .footer(
                div()
                    .text_sm()
                    .text_color(gpui::opaque_grey(1.0, 0.3))
                    .child("AniMoe v0.0.1-snapshot")
            )
    }
}
