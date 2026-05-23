
pub fn viewer_query<'a>() -> &'a str {
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


pub(crate) fn media_list_query<'a>() -> &'a str {
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