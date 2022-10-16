using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Premium pricing details.
/// </summary>
[DataContract]
internal class CatalogItemPremiumPricingResult
{
    /// <summary>
    /// The price of the item, for Roblox Premium users.
    /// </summary>
    [DataMember(Name = "premiumPriceInRobux")]
    public long? Price { get; set; }
}
