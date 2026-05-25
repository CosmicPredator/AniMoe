use std::{sync::Arc, time::Duration};

use anyhow::anyhow;
use gpui::{Asset, ImageCacheError, RenderImage};
use gpui_tokio::Tokio;

use crate::anilist::client::AniList;

#[derive(Clone, Hash)]
pub struct RemoteImage {
    pub url: String,
}

pub struct RemoteImageLoader;

impl Asset for RemoteImageLoader {
    type Source = RemoteImage;

    type Output = Result<Arc<RenderImage>, ImageCacheError>;

    fn load(
        source: Self::Source,
        cx: &mut gpui::App,
    ) -> impl Future<Output = Self::Output> + Send + 'static {
        let al_client = cx.global::<AniList>().clone();
        let task = Tokio::spawn_result(cx, async move {
            let response = al_client
                .client
                .get(&source.url)
                .timeout(Duration::from_secs(10))
                .send()
                .await?
                .error_for_status()?;

            let bytes = response.bytes().await?;

            let dynamic = image::load_from_memory(&bytes)?;
            let resized = dynamic.resize(130, 190, image::imageops::FilterType::Nearest);

            let mut rgba = resized.to_rgba8();
            for pixel in rgba.chunks_exact_mut(4) {
                pixel.swap(0, 2);
            }

            let frame = image::Frame::new(rgba);

            Ok(Arc::new(RenderImage::new(vec![frame])))
        });
        async move {
            task.await
                .map_err(|e| ImageCacheError::Other(Arc::new(anyhow!(e))))
        }
    }
}
