namespace Roblox.Api;

/// <summary>
/// Roblox OAuth scopes that exist.
/// </summary>
public class OAuthScope
{
    /// <summary>
    /// This scope is used for authenticating with Roblox.
    /// </summary>
    public const string OpenID = "openid";

    /// <summary>
    /// Scope for obtaining user information.
    /// </summary>
    /// <remarks>
    /// https://apis.roblox.com/oauth/v1/userinfo
    /// </remarks>
    public const string Profile = "profile";
}
