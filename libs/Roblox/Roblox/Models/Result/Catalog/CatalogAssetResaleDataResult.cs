using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Resale data from the economy API.
/// </summary>
/// <remarks>
/// https://economy.roblox.com/v1/assets/2470750640/resale-data
/// </remarks>
[DataContract]
internal class CatalogAssetResaleDataResult
{
    /// <summary>
    /// The count of how many of this item were available for purchase total.
    /// </summary>
    [DataMember(Name = "assetStock")]
    public long? TotalAvailable { get; set; }

    /// <summary>
    /// The count of how many of this item are remaining for sale.
    /// </summary>
    [DataMember(Name = "numberRemaining")]
    public long? NumberRemaining { get; set; }

    /// <summary>
    /// The recent average selling price of the item, if it is limited.
    /// </summary>
    [DataMember(Name = "recentAveragePrice")]
    public long? RecentAveragePrice { get; set; }

    /// <summary>
    /// The original price of the item, before it became resellable.
    /// </summary>
    [DataMember(Name = "originalPrice")]
    public long? OriginalPrice { get; set; }
}
