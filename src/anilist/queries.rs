pub(crate) fn q_viewer<'a>() -> &'a str {
    r#"
    query {
    	Viewer {
    		avatar {
    			medium
    		}
    		id
    		name
    	}
    }
    "#
}

pub(crate) fn q_media_list<'a>() -> &'a str {
    r#"
    query($id: Int, $type: MediaType) {
    	MediaListCollection(userId: $id, type: $type) {
    		lists {
                status
    			name
    			isCustomList
    			entries {
    				...MediaInfo
    			}
    		}
    	}
    }
    
    fragment MediaInfo on MediaList {
    	mediaId
    	media {
    		coverImage {
   			    large
    		}
    		title {
    			userPreferred
    		}
            nextAiringEpisode {
                episode
            }
            format
    		episodes
    		chapters
    		volumes
    	}
    	progress
    	progressVolumes
    	updatedAt
    }
    "#
}

pub(crate) fn m_update_media_list<'a>() -> &'a str {
    r#"
    mutation ($id: Int, $status: MediaListStatus, $progress: Int) {
      SaveMediaListEntry(mediaId: $id, progress: $progress, status: $status) {
        progress
        status
      }
    }
    "#
}
