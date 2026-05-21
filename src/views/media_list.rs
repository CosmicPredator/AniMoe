use gpui::{AppContext, Context, Entity, ParentElement, Render, Styled, Subscription, Window, div, px};
use gpui_component::{Icon, IconName, IndexPath, Sizable, StyledExt, button::{Button, ButtonVariants}, input::{Input, InputEvent, InputState}, select::{Select, SelectEvent, SelectState}};

use crate::utils::enums::MediaType;


pub struct MediaListPage {
    media_type: MediaType,
    _subscriptions: Vec<Subscription>,
    input_state: Entity<InputState>,
    select_state: Entity<SelectState<Vec<String>>>
}

impl MediaListPage {
    pub fn new(cx: &mut Context<Self>, window: &mut Window) -> Self {
        let input = cx.new(|cx| InputState::new(window, cx)
            .placeholder("Search anime...")
            .multi_line(false));
        
        let select_state = cx.new(|cx| {
            SelectState::new(
                vec!["Completed", "Watching", "Plans to Watch", "Paused", "Dropped"]
                    .iter().map(|f| String::from(*f)).collect(),
                None,
                window,
                cx
            )
        });

        let select_sub = cx.subscribe_in(&select_state, window, |view, state, event, window, cx| {
            match event {
                SelectEvent::Confirm(value) => {
                    if let Some(selected_value) = value {
                        println!("Selected: {:?}", selected_value);
                    } else {
                        println!("Selection cleared");
                    }
                }
            }
        });

        let input_sub = cx.subscribe_in(&input, window, |view, state, event, window, cx| {
            match event {
                InputEvent::Change => {
                    let text = state.read(cx).value();
                    println!("Input changed: {}", text);
                }
                InputEvent::PressEnter { secondary, shift } => {
                    println!("Enter pressed, secondary: {}", secondary);
                }
                InputEvent::Focus => println!("Input focused"),
                InputEvent::Blur => println!("Input blurred"),
            }
        });
        
        let subs = vec![select_sub, input_sub];
        Self { media_type: MediaType::Anime, _subscriptions: subs, input_state: input, select_state }
    }
}

impl Render for MediaListPage {
    fn render(&mut self, window: &mut gpui::Window, cx: &mut gpui::prelude::Context<Self>) -> impl gpui::prelude::IntoElement {
        div()
            .v_flex()
            .size_full()
            .ml(px(40.))
            .mr(px(40.))
            .mt(px(40.))
            .gap(px(20.))
            .child(
                div()
                    .text_3xl()
                    .font_semibold()
                    .child("Anime List")
            )
            .child(
                div()
                    .w_full()
                    .h_flex()
                    .justify_end()
                    .child(
                        div()
                            .h_flex()
                            .gap_2()
                            .child(
                                Input::new(&self.input_state)
                                    .w(px(250.0))
                                    .cleanable(true)
                            )
                            .child(
                                Button::new("search-btn")
                                    .icon(Icon::empty().path("./assets/search.svg"))
                            )
                    )
                    .child(div().flex_1())
                    .child(
                        div()
                            .h_flex()
                            .gap_2()
                            .child(
                                Select::new(&self.select_state)
                                    .w(px(200.0))
                                    .placeholder("Select a status")
                            )
                            .child(
                                Button::new("filter-btn")
                                    .icon(Icon::empty().path("./assets/list-filter.svg"))
                            )
                    )
            )
    }
}
