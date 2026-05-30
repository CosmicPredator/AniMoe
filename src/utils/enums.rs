use serde::Serialize;

#[allow(dead_code)]
#[derive(Clone, Copy, PartialEq, Eq, Serialize)]
#[serde()]
pub enum MediaType {
    ANIME,
    MANGA,
}
