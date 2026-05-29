/// Base URL for the service API.
pub const AL_URL: &str = "";

/// OAuth authorization endpoint used to initiate the login flow.
///
/// Users are redirected to this URL to grant access and obtain an
/// authorization code.
pub const AL_AUTH_URL: &str = "";

/// OAuth token endpoint used to exchange an authorization code
/// for an access token.
pub const AL_ACCESS_TOKEN_URL: &str = "";

/// OAuth client identifier issued to the application.
pub const CLIENT_ID: &str = "";

/// OAuth client secret issued to the application.
///
/// Keep this value secure and never expose it in client-side code
/// or public repositories.
pub const CLIENT_SECRET: &str = "";

/// OAuth redirect URI registered with the authorization server.
///
/// After successful authentication, the authorization server
/// redirects the user to this URI with the authorization code.
pub const REDIRECT_URI: &str = "";