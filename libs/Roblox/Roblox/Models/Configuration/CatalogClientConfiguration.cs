using System;
using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Configuration for the <see cref="ICatalogClient"/>.
/// </summary>
[DataContract(Name = "Catalog")]
internal class CatalogClientConfiguration
{
    /// <summary>
    /// How many assets to batch together for a single outbound request.
    /// </summary>
    /// <remarks>
    /// Set to one or below to disable batching.
    /// </remarks>
    public int AssetBatchSize { get; set; } = 500;

    /// <summary>
    /// How many bundles to batch together for a single outbound request.
    /// </summary>
    /// <remarks>
    /// Set to one or below to disable batching.
    /// </remarks>
    public int BundleBatchSize { get; set; } = 75;

    /// <summary>
    /// How many assets to batch together for a single outbound request for tags.
    /// </summary>
    /// <remarks>
    /// Set to one or below to disable batching.
    /// </remarks>
    public int AssetTagBatchSize { get; set; } = 50;

    /// <summary>
    /// Whether or not loading asset data should load resale data, for limited items.
    /// </summary>
    public bool ResaleDataEnabled { get; set; } = true;

    /// <summary>
    /// The minimum time that must pass between sending batched requests.
    /// </summary>
    public TimeSpan Throttle { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The maximum time to wait before sending a batched request.
    /// </summary>
    public TimeSpan MaxWaitTime { get; set; } = TimeSpan.FromSeconds(15);
}
