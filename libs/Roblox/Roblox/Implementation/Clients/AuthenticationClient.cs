using System;
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
public class AuthenticationClient : IAuthenticationClient
{
    private readonly HttpClient _HttpClient;
    private readonly string _Authorization;

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
    }

    /// <inheritdoc cref="IAuthenticationClient.LoginAsync"/>
    public async Task<LoginResult> LoginAsync(string code, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(_Authorization))
        {
            throw new ConfigurationException($"The app must have the Roblox.Authentication configuration filled in with {nameof(AuthenticationConfiguration.ClientId)} and {nameof(AuthenticationConfiguration.ClientSecret)} to use this method.");
        }

        var tokenRequest = new HttpRequestMessage(HttpMethod.Post, new Uri($"{RobloxDomain.Apis}/oauth/v1/token"));
        tokenRequest.Headers.Authorization = new AuthenticationHeaderValue("Basic", _Authorization);
        tokenRequest.Content = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["grant_type"] = "authorization_code",
            ["code"] = code
        });
        var tokenResponse = await _HttpClient.SendApiRequestAsync<OAuthTokenResult>(tokenRequest, "oauth/v1/token", cancellationToken);

        var userRequest = new HttpRequestMessage(HttpMethod.Get, new Uri($"{RobloxDomain.Apis}/oauth/v1/userinfo"));
        userRequest.Headers.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.AccessToken);
        var userResponse = await _HttpClient.SendApiRequestAsync<UserInfoResult>(userRequest, "oauth/v1/userinfo", cancellationToken);

        return new LoginResult
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = tokenResponse.RefreshToken,
            User = new UserResult
            {
                Id = userResponse.Id,
                Name = userResponse.Name,
                DisplayName = userResponse.DisplayName
            }
        };
    }
}
