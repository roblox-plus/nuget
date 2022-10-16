using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Api;

/// <summary>
/// A delegating handler for <see cref="HttpClient"/> to retry requests that fail with X-CSRF-Token failures.
/// </summary>
public class XsrfTokenHandler : DelegatingHandler
{
    private const string _HeaderName = "X-CSRF-Token";
    private const int _MaxAttempts = 3;
    private string _XsrfToken = "";

    /// <inheritdoc cref="DelegatingHandler.SendAsync"/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        HttpResponseMessage httpResponse = null;

        if (request.Method != HttpMethod.Get
            && request.RequestUri != null
            && request.RequestUri.Host.EndsWith(RobloxDomain.Value, StringComparison.InvariantCulture))
        {
            for (var i = 0; i < _MaxAttempts; i++)
            {
                if (!string.IsNullOrWhiteSpace(_XsrfToken))
                {
                    request.Headers.Remove(_HeaderName);
                    request.Headers.Add(_HeaderName, _XsrfToken);
                }

                httpResponse = await base.SendAsync(request, cancellationToken);
                if (!httpResponse.IsSuccessStatusCode && httpResponse.Headers.TryGetValues(_HeaderName, out var tokens))
                {
                    _XsrfToken = tokens.First();
                }
                else
                {
                    break;
                }
            }
        }
        else
        {
            httpResponse = await base.SendAsync(request, cancellationToken);
        }

        return httpResponse;
    }
}
