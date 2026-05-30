use std::{env, fs::File, io::BufReader, time::Duration};

use anyhow::{Context, anyhow};
use gpui::Global;
use log::debug;
use reqwest::{
    StatusCode,
    header::{AUTHORIZATION, HeaderMap, HeaderValue},
};
use serde::de::DeserializeOwned;
use serde_json::{Value, json};

use crate::{
    anilist::{
        media_list::MediaListResponse,
        queries::{m_update_media_list, q_media_list, q_viewer},
        token_callback::AccessTokenCallback,
        viewer::ViewerResponse,
    },
    utils::{
        constants::{AL_ACCESS_TOKEN_URL, AL_URL, CLIENT_ID, CLIENT_SECRET, REDIRECT_URI},
        enums::MediaType,
    },
};

#[derive(Clone)]
pub struct AniList {
    pub client: reqwest::Client,
    pub headers: HeaderMap,
}

impl Global for AniList {}

impl AniList {
    pub fn new() -> anyhow::Result<Self> {
        debug!("initializing http client");
        let access_token = env::var("AL_ACCESS_TOKEN");
        if access_token.is_err() {
            return Err(anyhow!("failed to fetch access token"));
        }

        let client = reqwest::Client::builder()
            .timeout(Duration::from_secs(10))
            .user_agent("AniMoe")
            .build()?;

        Ok(Self {
            client,
            headers: HeaderMap::new(),
        })
    }

    fn read_access_token(&self) -> anyhow::Result<AccessTokenCallback> {
        if let Some(config_dir) = dirs::config_local_dir() {
            let animoe_dir = config_dir.join("animoe");
            let config_file_path = animoe_dir.join("token.json");

            if !config_file_path.exists() {
                return Err(anyhow!("config file not found"));
            }

            let file = File::open(config_file_path)?;
            let reader = BufReader::new(file);

            let token_callback: AccessTokenCallback = serde_json::from_reader(reader)?;
            Ok(token_callback)
        } else {
            return Err(anyhow!("failed to access config dir"));
        }
    }

    async fn query<T>(&mut self, query: &str, variables: Option<Value>) -> anyhow::Result<T>
    where
        T: DeserializeOwned,
    {
        if !self.headers.contains_key(AUTHORIZATION) {
            let access_token = self
                .read_access_token()
                .map(|token_callback| format!("Bearer {}", token_callback.access_token))?;
            self.headers
                .append(AUTHORIZATION, HeaderValue::from_str(&access_token).unwrap());
        }

        let body = json!({
            "query": query,
            "variables": variables
        });

        debug!(
            "trying to fetch anilist for query: {}, variables: {:?}",
            query, variables
        );

        let response = self
            .client
            .post(AL_URL)
            .headers(self.headers.clone())
            .json(&body)
            .send()
            .await;

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

    pub async fn get_access_token(&self, auth_code: String) -> anyhow::Result<AccessTokenCallback> {
        let payload = json!({
            "grant_type": "authorization_code",
            "client_id": CLIENT_ID,
            "client_secret": CLIENT_SECRET,
            "redirect_uri": REDIRECT_URI,
            "code": auth_code
        });

        let request = self
            .client
            .post(AL_ACCESS_TOKEN_URL)
            .json(&payload)
            .send()
            .await
            .context(format!(
                "failed to get access_token for code: {}",
                auth_code
            ))?;

        let response = request.json::<AccessTokenCallback>().await?;
        Ok(response)
    }

    pub async fn fetch_viewer(&mut self) -> anyhow::Result<ViewerResponse> {
        debug!("initiating viewer query");
        let data: ViewerResponse = self
            .query(q_viewer(), None)
            .await
            .context("failed to call Viewer query")?;
        Ok(data)
    }

    pub async fn fetch_anime_list(
        &mut self,
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

    pub async fn update_episode_chapter(
        &mut self,
        media_id: i64,
        progress: i64,
    ) -> anyhow::Result<()> {
        debug!(
            "updating list progress for media id {} with progress {}",
            media_id, progress
        );
        let variables = json!({
            "id": media_id,
            "progress": progress
        });

        self.query::<Value>(m_update_media_list(), Some(variables))
            .await
            .context("failed to execute list progress update mutation")?;

        Ok(())
    }
}
