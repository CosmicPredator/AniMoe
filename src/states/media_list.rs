use std::cmp::Reverse;

use gpui::{AppContext, Context, Entity, SharedString, Subscription, Window};
use gpui_component::{
    input::{InputEvent, InputState},
    select::{SelectEvent, SelectState},
};

use crate::{anilist::media_list::Entry, states::master::MasterState, utils::enums::MediaType};

pub struct MediaListState {
    master_state: Entity<MasterState>,

    pub status_select_state: Entity<SelectState<Vec<SharedString>>>,
    pub search_input_state: Entity<InputState>,

    pub current_status: SharedString,
    current_search_query: SharedString,
    _current_media_type: MediaType,

    pub selected_list: Option<Vec<Entry>>,
    _subscriptions: Vec<Subscription>,
}

impl MediaListState {
    pub fn new(
        cx: &mut Context<Self>,
        win: &mut Window,
        master_state: Entity<MasterState>,
        media_type: MediaType,
    ) -> Self {
        let current_status = "Watching";

        let search_input_state = cx.new(|cx| {
            let placeholder_text = {
                if media_type == MediaType::ANIME {
                    "Search anime..."
                } else {
                    "Search manga..."
                }
            };
            InputState::new(win, cx).placeholder(placeholder_text)
        });

        let select_state = cx.new(|cx| SelectState::new(Vec::default(), None, win, cx));

        let mut subs = Vec::new();
        subs.push(cx.observe_in(&master_state, win, |this, _, win, cx| {
            this.update_status_select(cx, win);
            this.update_list(cx);
        }));

        subs.push(cx.subscribe_in(
            &select_state,
            win,
            |this, _state, event: &SelectEvent<Vec<SharedString>>, _window, cx| match event {
                SelectEvent::Confirm(value) => {
                    if let Some(selected_value) = value {
                        this.current_status = selected_value.clone();
                        this.update_list(cx);
                    }
                }
            },
        ));

        subs.push(cx.subscribe_in(
            &search_input_state,
            win,
            |this, state, event, _window, cx| {
                if let InputEvent::Change = event {
                    let text = state.read(cx).value();
                    this.current_search_query = text;
                    this.update_list(cx);
                }
            },
        ));

        Self {
            master_state,
            search_input_state,
            status_select_state: select_state,
            current_status: SharedString::new(current_status),
            current_search_query: SharedString::new(""),
            _current_media_type: media_type,
            selected_list: None,
            _subscriptions: subs,
        }
    }

    pub fn update_status_select(&self, cx: &mut Context<Self>, win: &mut Window) {
        let status_list: Vec<SharedString> = self
            .master_state
            .read(cx)
            .anime_list
            .as_ref()
            .map_or_else(Vec::new, |list| {
                list.iter()
                    .map(|entry| entry.name.clone())
                    .collect()
            });
        
        self.status_select_state.update(cx, |this, cx| {
            this.set_items(status_list, win, cx);
            this.set_selected_value(&self.current_status, win, cx);
            cx.notify();
        })
    }

    pub fn update_list(&mut self, cx: &mut Context<Self>) {
        let master_state = self.master_state.read(cx);
        let query = self.current_search_query.to_lowercase();

        self.selected_list = master_state
            .anime_list
            .as_ref()
            .and_then(|list| list.iter().find(|f| f.name == self.current_status))
            .map(|list| {
                let mut entries = list.entries.clone();
                entries.sort_by_key(|f| Reverse(f.updated_at));
                if !query.is_empty() {
                    entries
                        .retain(|f| f.media.title.user_preferred.to_lowercase().contains(&query));
                }
                entries
            });

        cx.notify();
    }
}
