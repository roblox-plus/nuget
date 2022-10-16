using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Prometheus;

namespace Roblox.Api;

/// <summary>
/// A handler for recording outbound network traffic to Roblox.
/// </summary>
internal class MetricsHandler : DelegatingHandler
{
    private static readonly Counter _RequestsCounter = Metrics.CreateCounter(
        "roblox_client_requests_per_second",
        "How many requests/second are being sent outbound to Roblox.",
        new[] { "domain", "endpoint", "status_code" });

    private static readonly Histogram _ExecutionTimeHistogram = Metrics.CreateHistogram(
        "roblox_client_execution_time",
        "How long requests are taking to send to Roblox.",
        new[] { "domain", "endpoint" });

    /// <inheritdoc cref="DelegatingHandler.SendAsync"/>
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var domain = request.RequestUri?.Host ?? string.Empty;
        if (!request.Options.TryGetValue(new HttpRequestOptionsKey<string>("endpoint"), out var endpoint))
        {
            endpoint = request.RequestUri?.AbsolutePath ?? string.Empty;
        }

        using var executionTime = _ExecutionTimeHistogram.WithLabels(domain, endpoint).NewTimer();

        try
        {
            var httpResponse = await base.SendAsync(request, cancellationToken);

            _RequestsCounter.WithLabels(domain, endpoint, $"{(int)httpResponse.StatusCode}").Inc();

            return httpResponse;
        }
        catch
        {
            _RequestsCounter.WithLabels(domain, endpoint).Inc();
            throw;
        }
    }
}
