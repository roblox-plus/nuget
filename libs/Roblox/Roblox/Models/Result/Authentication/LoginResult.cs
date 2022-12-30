using System.Runtime.Serialization;
using Roblox.Users;

namespace Roblox.Authentication;

/// <summary>
/// The result of logging in via OAuth 2.0.
/// </summary>
[DataContract]
public class LoginResult
{
    /// <summary>
    /// The access token, which can be used immediately to make requests.
    /// </summary>
    [DataMember(Name = "accessToken")]
    public string AccessToken { get; set; }

    /// <summary>
    /// The refresh token, which can be used to obtain an access token.
    /// </summary>
    [DataMember(Name = "refreshToken")]
    public string RefreshToken { get; set; }

    /// <summary>
    /// The user we logged in as.
    /// </summary>
    [DataMember(Name = "user")]
    public UserResult User { get; set; }
}
