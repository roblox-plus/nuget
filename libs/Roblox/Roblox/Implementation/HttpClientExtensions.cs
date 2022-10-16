using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Prometheus;

namespace Roblox.Api;

internal static class HttpClientExtensions
{
    private static readonly Counter _RequestsCounter = Metrics.CreateCounter(
        "roblox_client_requests_per_second",
        "How many requests/second are being sent outbound to Roblox.",
        new[] { "domain", "endpoint", "status_code" });

    private static readonly Histogram _ExecutionTimeHistogram = Metrics.CreateHistogram(
        "roblox_client_execution_time",
        "How long requests are taking to send to Roblox.",
        new[] { "domain", "endpoint" });

    /// <summary>
    /// Sends an HTTP request to Roblox, and attempts to parse the result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/> to send the request with.</param>
    /// <param name="httpMethod">The <see cref="HttpMethod"/> for the request.</param>
    /// <param name="domain">The <see cref="RobloxDomain"/> to send the request to.</param>
    /// <param name="path">The request path.</param>
    /// <param name="queryParameters">The query parameters for the <see cref="Uri"/>.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <typeparamref name="TResult"/>.</returns>
    /// <exception cref="RobloxApiException">
    /// The Roblox request failed.
    /// </exception>
    public static Task<TResult> SendApiRequestAsync<TResult>(this HttpClient httpClient, HttpMethod httpMethod, string domain, FormattableString path, IReadOnlyDictionary<string, string> queryParameters, CancellationToken cancellationToken)
        where TResult : class
    {
        var url = RobloxDomain.Build(domain, path.ToString(), queryParameters);
        var httpRequest = new HttpRequestMessage(httpMethod, url);
        return httpClient.SendApiRequestAsync<TResult>(httpRequest, path.Format, cancellationToken);
    }

    /// <summary>
    /// Sends an HTTP request to Roblox, and attempts to parse the result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <typeparam name="TRequest">The request body type.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/> to send the request with.</param>
    /// <param name="httpMethod">The <see cref="HttpMethod"/> for the request.</param>
    /// <param name="domain">The <see cref="RobloxDomain"/> to send the request to.</param>
    /// <param name="path">The request path.</param>
    /// <param name="queryParameters">The query parameters for the <see cref="Uri"/>.</param>
    /// <param name="requestBody">The JSON request body.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <typeparamref name="TResult"/>.</returns>
    /// <exception cref="RobloxApiException">
    /// The Roblox request failed.
    /// </exception>
    public static Task<TResult> SendApiRequestAsync<TRequest, TResult>(this HttpClient httpClient, HttpMethod httpMethod, string domain, FormattableString path, IReadOnlyDictionary<string, string> queryParameters, TRequest requestBody, CancellationToken cancellationToken)
        where TResult : class
        where TRequest : class
    {
        var url = RobloxDomain.Build(domain, path.ToString(), queryParameters);
        var httpRequest = new HttpRequestMessage(httpMethod, url);
        httpRequest.Content = new StringContent(JsonConvert.SerializeObject(requestBody));
        httpRequest.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");

        return httpClient.SendApiRequestAsync<TResult>(httpRequest, path.Format, cancellationToken);
    }

    /// <summary>
    /// Sends an HTTP request to Roblox, and attempts to parse the result.
    /// </summary>
    /// <typeparam name="TResult">The result type.</typeparam>
    /// <param name="httpClient">The <see cref="HttpClient"/> to send the request with.</param>
    /// <param name="httpRequest">The <see cref="HttpRequestMessage"/> to send.</param>
    /// <param name="endpoint">The endpoint which the request was sent to, for telemetry.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <typeparamref name="TResult"/>.</returns>
    /// <exception cref="RobloxApiException">
    /// The Roblox request failed.
    /// </exception>
    private static async Task<TResult> SendApiRequestAsync<TResult>(this HttpClient httpClient, HttpRequestMessage httpRequest, string endpoint, CancellationToken cancellationToken)
        where TResult : class
    {
        HttpStatusCode? statusCode = null;
        var domain = httpRequest.RequestUri?.Host ?? string.Empty;

        try
        {
            using var timer = _ExecutionTimeHistogram.WithLabels(domain, endpoint).NewTimer();
            var response = await httpClient.SendAsync(httpRequest, cancellationToken);
            statusCode = response.StatusCode;

            var responseBody = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<TResult>(responseBody);
            }

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                throw new RobloxUnauthenticatedException(httpRequest, response);
            }

            throw new RobloxApiException(httpRequest, response);
        }
        catch (RobloxApiException)
        {
            throw;
        }
        catch (JsonException ex)
        {
            throw new RobloxApiException($"Failed to parse response body from Roblox.\n\tUrl: {httpRequest.RequestUri}", ex);
        }
        catch (Exception ex)
        {
            throw new RobloxApiException($"An unknown exception occurred while sending the request to Roblox.\n\tUrl: {httpRequest.RequestUri}", ex);
        }
        finally
        {
            if (statusCode.HasValue)
            {
                _RequestsCounter.WithLabels(domain, endpoint, $"{(int)statusCode}").Inc();
            }
            else
            {
                _RequestsCounter.WithLabels(domain, endpoint).Inc();
            }
        }
    }

    /// <summary>
    /// Initializes a new model class intended to host settings about client.
    /// </summary>
    /// <typeparam name="TConfiguration">The client settings class.</typeparam>
    /// <param name="configuration">An <seealso cref="IConfiguration"/>.</param>
    /// <param name="settings">The settings instance, with default values set.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="configuration"/>
    /// - <paramref name="settings"/>
    /// </exception>
    /// <exception cref="ArgumentException">
    /// - <typeparamref name="TConfiguration"/> is missing <see cref="DataContractAttribute"/> with <see cref="DataContractAttribute.Name"/> set.
    /// </exception>
    internal static string BindClientConfiguration<TConfiguration>(this IConfiguration configuration, TConfiguration settings)
        where TConfiguration : class
    {
        if (configuration == null)
        {
            throw new ArgumentNullException(nameof(configuration));
        }

        if (settings == null)
        {
            throw new ArgumentNullException(nameof(settings));
        }

        var dataContract = typeof(TConfiguration).GetCustomAttribute<DataContractAttribute>();
        if (string.IsNullOrWhiteSpace(dataContract?.Name))
        {
            throw new ArgumentException($"{nameof(TConfiguration)} expected to have {nameof(DataContractAttribute)} with {nameof(DataContractAttribute.Name)} set to the client name.", nameof(TConfiguration));
        }

        var robloxConfiguration = configuration.GetSection("Roblox");
        var clientsConfiguration = robloxConfiguration.GetSection("Clients");
        var clientConfiguration = clientsConfiguration.GetSection(dataContract.Name);
        clientConfiguration.Bind(settings);

        return dataContract.Name;
    }
}
