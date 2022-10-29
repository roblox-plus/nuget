using System;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Roblox.Api;

/// <summary>
/// Default configuration for the <see cref="HttpClient"/> used by the Roblox clients.
/// </summary>
[DataContract(Name = "HttpClient")]
internal class HttpClientConfiguration
{
    /// <summary>
    /// The default request timeout.
    /// </summary>
    /// <remarks>
    /// How long to wait for a response for requests sent to Roblox.
    /// </remarks>
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(15);

    /// <summary>
    /// The default User-Agent header to use.
    /// </summary>
    public string UserAgent { get; set; } = "Roblox NuGet (https://www.nuget.org/packages/Roblox)";
}
