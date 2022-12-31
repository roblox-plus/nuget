using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Authentication;

/// <summary>
/// The result of requesting for an access token/refresh token.
/// </summary>
/// <remarks>
/// See also: https://devforum.roblox.com/t/introduction-to-oauth-20/1940062#tokens-2
/// </remarks>
[DataContract]
internal class OAuthTokenResult
{
    /// <summary>
    /// We have some leeway on the expiration, to account for latency, and the ability to actually use the token.
    /// </summary>
    /// <remarks>
    /// This exists to ensure when the consumers of this library say "is the token expired?" they can use the token
    /// for up to this much extra time, to ensure that when they send their requests with it, they will succeed.
    /// </remarks>
    private static readonly TimeSpan _ExpirationLeeway = TimeSpan.FromMinutes(1);
    private ISet<string> _Scopes = new HashSet<string>();

    /// <summary>
    /// The Bearer token, which can be used to send requests to Roblox on behalf of the user.
    /// </summary>
    [DataMember(Name = "access_token")]
    public string AccessToken { get; set; }

    /// <summary>
    /// The refresh token, which can be used to obtain an <see cref="AccessToken"/>.
    /// </summary>
    /// <remarks>
    /// As documented, this should last for up to 6 months.
    /// But it is also mutable, and a new one will be returned every refresh.
    /// </remarks>
    [DataMember(Name = "refresh_token")]
    public string RefreshToken { get; set; }

    /// <summary>
    /// How long, in seconds, the access token will be valid for.
    /// </summary>
    /// <remarks>
    /// This property exists for deserialization.
    /// </remarks>
    [DataMember(Name = "expires_in")]
    public double ExpiresIn
    {
        private get
        {
            var expiration = AccessTokenExpiration - DateTime.UtcNow;
            return expiration.TotalSeconds;
        }
        set => AccessTokenExpiration = DateTime.UtcNow.AddSeconds(value - _ExpirationLeeway.TotalSeconds);
    }

    /// <summary>
    /// When the access token will expire.
    /// </summary>
    [DataMember(Name = "expiration")]
    public DateTime AccessTokenExpiration { get; private set; }

    /// <summary>
    /// The scopes associated with the token.
    /// </summary>
    /// <remarks>
    /// This property exists for deserialization.
    /// </remarks>
    [DataMember(Name = "scope")]
    public string RawScopes
    {
        get => string.Join(' ', _Scopes);
        set
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                _Scopes = new HashSet<string>();
            }
            else
            {
                _Scopes = new HashSet<string>(value.Split(' '), StringComparer.OrdinalIgnoreCase);
            }
        }
    }

    /// <summary>
    /// The scopes associated with the token.
    /// </summary>
    [IgnoreDataMember]
    public ISet<string> Scopes => _Scopes;
}
