using System;
using System.Runtime.Serialization;

namespace Roblox.Thumbnails;

/// <summary>
/// Configuration for the <seealso cref="IThumbnailsClient"/>.
/// </summary>
[DataContract(Name = "Thumbnails")]
public class ThumbnailClientConfiguration
{
    /// <summary>
    /// The size to request for all thumbnails.
    /// </summary>
    public string ThumbnailSize { get; set; } = "420x420";

    /// <summary>
    /// How many thumbnails to batch together for a single request.
    /// </summary>
    public int BatchSize { get; set; } = 100;

    /// <summary>
    /// The minimum time that must pass between sending batched requests.
    /// </summary>
    public TimeSpan Throttle { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The maximum time to wait before sending a batched request.
    /// </summary>
    public TimeSpan MaxWaitTime { get; set; } = TimeSpan.FromSeconds(15);
}
