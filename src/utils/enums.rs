use serde::Serialize;

#[derive(Clone, Copy, PartialEq, Eq, Serialize)]
#[serde()]
pub enum MediaType {
    ANIME,
    MANGA,
}
