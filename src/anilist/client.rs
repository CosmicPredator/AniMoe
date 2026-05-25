use std::{env, str::Bytes, time::Duration};

use anyhow::{Context, anyhow};
use futures::TryFutureExt;
use gpui::Global;
use log::debug;
use reqwest::{
    Response, StatusCode,
    header::{AUTHORIZATION, HeaderMap, HeaderValue},
};
use serde::de::DeserializeOwned;
use serde_json::{Value, json};

use crate::{
    anilist::{
        media_list::MediaListResponse,
        queries::{m_update_media_list, q_media_list, q_viewer},
        viewer::ViewerResponse,
    },
    utils::{constants::AL_URL, enums::MediaType},
};

#[derive(Clone)]
pub struct AniList {
    pub client: reqwest::Client,
}

impl Global for AniList {}

impl AniList {
    pub fn new() -> anyhow::Result<Self> {
        debug!("initializing http client");
        let access_token = env::var("AL_ACCESS_TOKEN");
        if access_token.is_err() {
            return Err(anyhow!("failed to fetch access token"));
        }

        let mut headers = HeaderMap::new();
        let access_token = format!("Bearer {}", access_token.unwrap());
        headers.append(AUTHORIZATION, HeaderValue::from_str(&access_token).unwrap());

        let client = reqwest::Client::builder()
            .timeout(Duration::from_secs(10))
            .default_headers(headers)
            .user_agent("AniMoe")
            .build()?;

        Ok(Self { client })
    }

    async fn query<T>(&self, query: &str, variables: Option<Value>) -> anyhow::Result<T>
    where
        T: DeserializeOwned,
    {
        let body = json!({
            "query": query,
            "variables": variables
        });

        debug!(
            "trying to fetch anilist for query: {}, variables: {:?}",
            query, variables
        );
        let response = self.client.post(AL_URL).json(&body).send().await;

        match response {
            Ok(result) => {
                let deserialized = result
                    .json::<T>()
                    .await
                    .context(format!("failed to deserialize json. query: {}", query))?;

                Ok(deserialized)
            }
            Err(err) => Err(anyhow!(
                "failed to make http request. status code: {}",
                err.status().unwrap_or(StatusCode::INTERNAL_SERVER_ERROR)
            )),
        }
    }

    pub async fn fetch_viewer(&self) -> anyhow::Result<ViewerResponse> {
        debug!("initiating viewer query");
        let data: ViewerResponse = self
            .query(q_viewer(), None)
            .await
            .context("failed to call Viewer query")?;
        Ok(data)
    }

    pub async fn fetch_anime_list(
        &self,
        user_id: i64,
        media_type: MediaType,
    ) -> anyhow::Result<MediaListResponse> {
        debug!("initiating anime list query");
        let variables = json!({
            "id": user_id,
            "type": media_type
        });

        let data = self
            .query(q_media_list(), Some(variables))
            .await
            .context("failed to execute anime list query")?;
        Ok(data)
    }

    pub async fn update_episode_chapter(&self, media_id: i64, progress: i64) -> anyhow::Result<()> {
        debug!("updating list progress for media id {} with progress {}", media_id, progress);
        let variables = json!({
            "id": media_id,
            "progress": progress
        });

        self
            .query::<Value>(m_update_media_list(), Some(variables))
            .await
            .context("failed to execute list progress update mutation")?;

        Ok(())
    }
}
