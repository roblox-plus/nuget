using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Prometheus;
using Roblox.Api;
using Roblox.Users;

namespace Roblox.Authentication;

/// <inheritdoc cref="IAuthenticationClient"/>
public class AuthenticationClient : IAuthenticationClient, IDisposable
{
    private const string _LoginGrantType = "authorization_code";
    private const string _RefreshTokenGrantType = "refresh_token";

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

    private static readonly Counter _AuthenticationFailureCounter = Metrics.CreateCounter(
        name: "authentication_failure",
        help: "Number of Roblox OAuth authentication attempts that fail.",
        labelNames: new[] { "grant_type" });

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
        if (string.IsNullOrWhiteSpace(code))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(code));
        }

        if (string.IsNullOrWhiteSpace(_Authorization))
        {
            throw new ConfigurationException($"The app must have the Roblox.Authentication configuration filled in with {nameof(AuthenticationConfiguration.ClientId)} and {nameof(AuthenticationConfiguration.ClientSecret)} to use this method.");
        }

        try
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{RobloxDomain.Apis}/oauth/v1/token"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", _Authorization);
            httpRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = _LoginGrantType,
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
        catch (Exception)
        {
            _AuthenticationFailureCounter.WithLabels(_LoginGrantType).Inc();
            throw;
        }
    }

    /// <inheritdoc cref="IAuthenticationClient.RefreshAsync"/>
    public Task<LoginResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(refreshToken));
        }

        if (string.IsNullOrWhiteSpace(_Authorization))
        {
            throw new ConfigurationException($"The app must have the Roblox.Authentication configuration filled in with {nameof(AuthenticationConfiguration.ClientId)} and {nameof(AuthenticationConfiguration.ClientSecret)} to use this method.");
        }

        return _RefreshCache.GetOrAdd(refreshToken, t => UncachedRefreshAsync(t, cancellationToken));
    }

    /// <inheritdoc cref="IAuthenticationClient.LogoutAsync"/>
    public async Task LogoutAsync(string token, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(token))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(token));
        }

        // If it's in there... remove it.
        _RefreshCache.TryRemove(token, out _);

        var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{RobloxDomain.Apis}/oauth/v1/token/revoke"));
        httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", _Authorization);
        httpRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["token"] = token
        });

        await _HttpClient.SendApiRequestAsync<OAuthTokenResult>(httpRequest, "oauth/v1/token/revoke", cancellationToken);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _CacheClearTimer?.Dispose();
    }

    private async Task<LoginResult> UncachedRefreshAsync(string refreshToken, CancellationToken cancellationToken)
    {
        try
        {
            var httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{RobloxDomain.Apis}/oauth/v1/token"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", _Authorization);
            httpRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["grant_type"] = _RefreshTokenGrantType,
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
        catch (RobloxApiException e) when (e.StatusCode == HttpStatusCode.BadRequest && e.ErrorCode == "invalid_grant")
        {
            throw new RobloxUnauthenticatedException(e);
        }
        catch (Exception)
        {
            _AuthenticationFailureCounter.WithLabels(_RefreshTokenGrantType).Inc();
            throw;
        }
    }

    private async Task<UserResult> GetUserDataAsync(OAuthTokenResult authenticationResult, CancellationToken cancellationToken)
    {
        HttpRequestMessage httpRequest;
        string requestPath;

        if (authenticationResult.Scopes.Contains(OAuthScope.Profile))
        {
            requestPath = "oauth/v1/userinfo";
            httpRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{RobloxDomain.Apis}/{requestPath}"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", authenticationResult.AccessToken);
        }
        else
        {
            requestPath = "oauth/v1/token/introspect";
            httpRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{RobloxDomain.Apis}/{requestPath}"));
            httpRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", _Authorization);
            httpRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                ["token"] = authenticationResult.AccessToken
            });
        }

        var response = await _HttpClient.SendApiRequestAsync<UserInfoResult>(httpRequest, requestPath, cancellationToken);
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
