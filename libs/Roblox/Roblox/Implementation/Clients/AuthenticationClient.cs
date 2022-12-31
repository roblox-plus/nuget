using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Roblox.Api;
using Roblox.Users;

namespace Roblox.Authentication;

/// <inheritdoc cref="IAuthenticationClient"/>
public class AuthenticationClient : IAuthenticationClient, IDisposable
{
    /// <summary>
    /// A dictionary, keyed by refresh token, of tokens that we have already refreshed.
    /// </summary>
    /// <remarks>
    /// We cache the tokens that we have already refreshed here, in case we attempt to refresh them again.
    /// We do this because once we use a refresh token it cannot be refreshed again.
    /// </remarks>
    private readonly ConcurrentDictionary<string, Task<LoginResult>> _RefreshCache = new(StringComparer.OrdinalIgnoreCase);
    private readonly TimeSpan _CachePurgeInterval = TimeSpan.FromSeconds(30);
    private readonly HttpClient _HttpClient;
    private readonly string _Authorization;
    private readonly Timer _CacheClearTimer;

    /// <summary>
    /// Initializes a new <seealso cref="AuthenticationClient"/>.
    /// </summary>
    /// <param name="httpClient">The <seealso cref="HttpClient"/> to use to make the requests.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// - <paramref name="configuration"/>
    /// </exception>
    public AuthenticationClient(HttpClient httpClient, IConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        var authenticationSettings = new AuthenticationConfiguration();
        var robloxConfiguration = configuration.GetSection("Roblox");
        var authenticationConfiguration = robloxConfiguration.GetSection("Authentication");
        authenticationConfiguration.Bind(authenticationSettings);

        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        _Authorization = authenticationSettings.Authorization;
        _CacheClearTimer = new Timer(PurgeCache, null, _CachePurgeInterval, _CachePurgeInterval);
    }

    /// <inheritdoc cref="IAuthenticationClient.LoginAsync"/>
    public async Task<LoginResult> LoginAsync(string code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_Authorization))
        {
            throw new ConfigurationException($"The app must have the Roblox.Authentication configuration filled in with {nameof(AuthenticationConfiguration.ClientId)} and {nameof(AuthenticationConfiguration.ClientSecret)} to use this method.");
        }

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{RobloxDomain.Apis}/oauth/v1/token"));
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", _Authorization);
        httpRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code
        });
        var httpResponse = await _HttpClient.SendApiRequestAsync<OAuthTokenResult>(httpRequest, "oauth/v1/token", cancellationToken);
        var user = await GetUserDataAsync(httpResponse, cancellationToken);

        return new LoginResult
        {
            AccessToken = httpResponse.AccessToken,
            RefreshToken = httpResponse.RefreshToken,
            AccessTokenExpiration = httpResponse.AccessTokenExpiration,
            Scopes = httpResponse.Scopes,
            User = user
        };
    }

    /// <inheritdoc cref="IAuthenticationClient.RefreshAsync"/>
    public Task<LoginResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_Authorization))
        {
            throw new ConfigurationException($"The app must have the Roblox.Authentication configuration filled in with {nameof(AuthenticationConfiguration.ClientId)} and {nameof(AuthenticationConfiguration.ClientSecret)} to use this method.");
        }

        return _RefreshCache.GetOrAdd(refreshToken, t => UncachedRefreshAsync(t, cancellationToken));
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _CacheClearTimer?.Dispose();
    }

    private async Task<LoginResult> UncachedRefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{RobloxDomain.Apis}/oauth/v1/token"));
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", _Authorization);
        httpRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "refresh_token",
            ["refresh_token"] = refreshToken
        });
        var httpResponse = await _HttpClient.SendApiRequestAsync<OAuthTokenResult>(httpRequest, "oauth/v1/token", cancellationToken);
        var user = await GetUserDataAsync(httpResponse, cancellationToken);

        return new LoginResult
        {
            AccessToken = httpResponse.AccessToken,
            RefreshToken = httpResponse.RefreshToken,
            AccessTokenExpiration = httpResponse.AccessTokenExpiration,
            Scopes = httpResponse.Scopes,
            User = user
        };
    }

    private async Task<UserResult> GetUserDataAsync(OAuthTokenResult authenticationResult, CancellationToken cancellationToken)
    {
        // HACK: We're taking advantage of that userinfo and token/introspect both return the user ID as "sub", so we can deserialize it into the same model.
        var userInfoPath = authenticationResult.Scopes.Contains(OAuthScope.Profile) ? "oauth/v1/userinfo" : "oauth/v1/token/introspect";
        var httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{RobloxDomain.Apis}/{userInfoPath}"));
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);

        var response = await _HttpClient.SendApiRequestAsync<UserInfoResult>(httpRequest, userInfoPath, cancellationToken);

        return new UserResult
        {
            Id = response.Id,
            Name = response.Name,
            DisplayName = response.DisplayName
        };
    }

    private void PurgeCache(object state)
    {
        foreach (var (refreshToken, result) in _RefreshCache.ToArray())
        {
            if (result.IsFaulted)
            {
                // Don't keep the failed refreshes around.
                _RefreshCache.TryRemove(refreshToken, out _);
                continue;
            }

            if (result.IsCompletedSuccessfully && result.Result.AccessTokenExpiration < DateTime.UtcNow)
            {
                // The access token for this one is already expired, and for now, we're using this as the cache expiry period.
                _RefreshCache.TryRemove(refreshToken, out _);
            }
        }
    }
}
