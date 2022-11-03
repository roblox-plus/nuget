using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Bundle product information.
/// </summary>
[DataContract]
internal class CatalogBundleProductResult
{
    /// <summary>
    /// The product ID.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// Whether or not the bundle is free.
    /// </summary>
    [DataMember(Name = "isPublicDomain")]
    public bool Free { get; set; }

    /// <summary>
    /// Whether or not the bundle is for sale.
    /// </summary>
    [DataMember(Name = "isForSale")]
    public bool ForSale { get; set; }

    /// <summary>
    /// The price of the item.
    /// </summary>
    [DataMember(Name = "priceInRobux")]
    public long? Price { get; set; }
}
