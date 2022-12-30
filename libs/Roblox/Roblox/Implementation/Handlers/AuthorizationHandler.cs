using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Roblox.Authentication;

namespace Roblox.Api;

/// <summary>
/// A delegating handler for <see cref="HttpClient"/> to add the Roblox client credentials to outbound requests.
/// </summary>
public class AuthorizationHandler : DelegatingHandler
{
    private readonly string _AuthorizationHeader;

    /// <summary>
    /// Initializes a new <see cref="AuthorizationHandler"/>.
    /// </summary>
    /// <param name="configuration">The <see cref="AuthenticationConfiguration"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="configuration"/>
    /// </exception>
    public AuthorizationHandler(AuthenticationConfiguration configuration)
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (string.IsNullOrWhiteSpace(configuration.ClientId))
        {
            throw new ArgumentException($"{nameof(configuration.ClientId)} must be set to use the {nameof(AuthorizationHandler)}", nameof(configuration));
        }

        if (string.IsNullOrWhiteSpace(configuration.ClientSecret))
        {
            throw new ArgumentException($"{nameof(configuration.ClientSecret)} must be set to use the {nameof(AuthorizationHandler)}", nameof(configuration));
        }

        _AuthorizationHeader = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{configuration.ClientId}:{configuration.ClientSecret}"));
    }

    /// <inheritdoc cref="DelegatingHandler.SendAsync"/>
    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.RequestUri != null
            && request.RequestUri.Host.EndsWith(RobloxDomain.Value, StringComparison.InvariantCulture)
            && request.Headers.Authorization == null)
        {
            request.Headers.Authorization = new AuthenticationHeaderValue("Basic", _AuthorizationHeader);
        }

        return base.SendAsync(request, cancellationToken);
    }
}
