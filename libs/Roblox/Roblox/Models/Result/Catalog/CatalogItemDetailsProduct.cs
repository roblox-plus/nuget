using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Virtual economy product details about the item.
/// </summary>
[DataContract]
public class CatalogItemDetailsProduct
{
    /// <summary>
    /// The product ID.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The price of the item.
    /// </summary>
    [DataMember(Name = "price")]
    public long? Price { get; set; }

    /// <summary>
    /// The price of the item for users with Roblox Premium.
    /// </summary>
    [DataMember(Name = "premiumPrice")]
    public long? PremiumPrice { get; set; }

    /// <summary>
    /// Whether or not the item is limited.
    /// </summary>
    [DataMember(Name = "limited")]
    public bool Limited { get; set; }

    /// <summary>
    /// The lowest selling price of the item, if it is limited.
    /// </summary>
    [DataMember(Name = "lowestPrice")]
    public long? LowestPrice { get; set; }

    /// <summary>
    /// The recent average selling price of the item, if it is limited.
    /// </summary>
    [DataMember(Name = "recentAveragePrice")]
    public long? RecentAveragePrice { get; set; }

    /// <summary>
    /// The count of how many of this item are remaining for sale.
    /// </summary>
    [DataMember(Name = "remaining", EmitDefaultValue = false)]
    public long? CountRemaining { get; set; }

    /// <summary>
    /// The count of how many of this item were available for purchase total.
    /// </summary>
    [DataMember(Name = "totalAvailable", EmitDefaultValue = false)]
    public long? TotalAvailable { get; set; }
}
