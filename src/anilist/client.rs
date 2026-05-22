use std::{env, time::Duration};

use anyhow::Ok;
use reqwest::{Client, header::{HeaderMap, HeaderValue}};
use serde_json::json;

use crate::{anilist::{queries::viewer_query, viewer::ViewerResponse}, utils::constants::AL_URL};


pub struct AniList {
    client: Client
}

impl AniList {
    pub fn new() -> anyhow::Result<Self> {
        let mut headers = HeaderMap::new();
        let access_token = format!("Bearer {}", env::var("AL_ACCESS_TOKEN").unwrap());
        headers.append("Authorization", HeaderValue::from_str(&access_token).unwrap());
        
        let client = Client::builder()
                .timeout(Duration::from_secs(10))
                .default_headers(headers)
                .user_agent("AniMoe")
                .build()?;
        Ok(Self { client })
    }

    pub async fn get_viewer(&self) -> anyhow::Result<ViewerResponse> {
        let body = json!({
            "query": viewer_query(),
            "variables": {}
        });

        let response = self.client
            .post(AL_URL)
            .json(&body)
            .send()
            .await?;

        let data: ViewerResponse = response.json().await?;
        Ok(data)
    }
}