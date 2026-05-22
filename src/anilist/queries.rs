
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
