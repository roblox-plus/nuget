using System.Collections.Generic;
using System.Runtime.Serialization;
using Roblox.Api;

namespace Roblox.Catalog;

/// <summary>
/// Catalog item details, from the catalog API.
/// </summary>
/// <remarks>
/// https://catalog.roblox.com/v1/catalog/items/details
/// </remarks>
[DataContract]
internal class CatalogItemDetailsResult
{
    /// <summary>
    /// The ID of the item.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The item type.
    /// </summary>
    [DataMember(Name = "itemType")]
    public string Type { get; set; }

    /// <summary>
    /// The <see cref="AssetType"/> ID.
    /// </summary>
    [DataMember(Name = "assetType")]
    public int? AssetTypeId { get; set; }

    /// <summary>
    /// The bundle type.
    /// </summary>
    [DataMember(Name = "bundleType")]
    public string BundleType { get; set; }

    /// <summary>
    /// The name of the item.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The item description.
    /// </summary>
    [DataMember(Name = "description")]
    public string Description { get; set; }

    /// <summary>
    /// The ID of the product associated with the item.
    /// </summary>
    [DataMember(Name = "productId")]
    public long? ProductId { get; set; }

    /// <summary>
    /// The item status.
    /// </summary>
    [DataMember(Name = "itemStatus")]
    public ISet<string> Statuses { get; set; }

    /// <summary>
    /// The item restrictions.
    /// </summary>
    [DataMember(Name = "itemRestrictions")]
    public ISet<string> Restrictions { get; set; }

    /// <summary>
    /// The ID of the creator of the item.
    /// </summary>
    [DataMember(Name = "creatorTargetId")]
    public long CreatorId { get; set; }

    /// <summary>
    /// The creator type.
    /// </summary>
    [DataMember(Name = "creatorType")]
    public string CreatorType { get; set; }

    /// <summary>
    /// The name of the creator of the item.
    /// </summary>
    [DataMember(Name = "creatorName")]
    public string CreatorName { get; set; }

    /// <summary>
    /// The cost of the item, in Robux.
    /// </summary>
    [DataMember(Name = "price")]
    public long? Price { get; set; }

    /// <summary>
    /// The lowest price for sale of the item, if the item is limited.
    /// </summary>
    [DataMember(Name = "lowestPrice")]
    public long? LowestPrice { get; set; }

    /// <summary>
    /// The count of how many of this item are remaining for sale.
    /// </summary>
    [DataMember(Name = "unitsAvailableForConsumption")]
    public long? CountRemaining { get; set; }

    /// <summary>
    /// Premium user pricing information.
    /// </summary>
    [DataMember(Name = "premiumPricing")]
    public CatalogItemPremiumPricingResult PremiumPricing { get; set; }
}
