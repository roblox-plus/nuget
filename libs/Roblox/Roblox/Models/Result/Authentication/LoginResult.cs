using System;
using System.Collections.Generic;
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
    [DataMember(Name = "access_token")]
    public string AccessToken { get; internal set; }

    /// <summary>
    /// The refresh token, which can be used to obtain an access token.
    /// </summary>
    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; internal set; }

    /// <summary>
    /// When the access token will expire.
    /// </summary>
    [DataMember(Name = "expiration")]
    public DateTime AccessTokenExpiration { get; internal set; }

    /// <summary>
    /// The user we logged in as.
    /// </summary>
    [DataMember(Name = "user")]
    public UserResult User { get; internal set; }

    /// <summary>
    /// The scopes associated with the token.
    /// </summary>
    /// <remarks>
    /// This property exists for serialization.
    /// </remarks>
    [DataMember(Name = "scopes")]
    public string RawScopes => string.Join(' ', Scopes);

    /// <summary>
    /// The scopes associated with the token.
    /// </summary>
    [IgnoreDataMember]
    public ISet<string> Scopes { get; internal set; }
}
