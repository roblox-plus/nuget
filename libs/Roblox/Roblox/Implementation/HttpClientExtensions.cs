using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Roblox.Api;

internal static class HttpClientExtensions
{
    /// <summary>
    /// Sends an HTTP request to Roblox, and attempts to parse the result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/> to send the request with.</param>
    /// <param name="httpMethod">The <see cref="HttpMethod"/> for the request.</param>
    /// <param name="url">The request <see cref="Uri"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <typeparamref name="TResult"/>.</returns>
    /// <exception cref="RobloxApiException">
    /// The Roblox request failed.
    /// </exception>
    public static async Task<TResult> SendApiRequestAsync<TResult>(this HttpClient httpClient, HttpMethod httpMethod, Uri url, CancellationToken cancellationToken)
        where TResult : class
    {
        try
        {
            var request = new HttpRequestMessage(httpMethod, url);
            var response = await httpClient.SendAsync(request, cancellationToken);
            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResult>(responseBody);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new RobloxUnauthenticatedException(request, response);
            }

            throw new RobloxApiException(request, response);
        }
        catch (RobloxApiException)
        {
            throw;
        }
        catch (JsonException ex)
        {
            throw new RobloxApiException($"Failed to parse response body from Roblox.\n\tUrl: {url}", ex);
        }
        catch (Exception ex)
        {
            throw new RobloxApiException($"An unknown exception occurred while sending the request to Roblox.\n\tUrl: {url}", ex);
        }
    }
}
