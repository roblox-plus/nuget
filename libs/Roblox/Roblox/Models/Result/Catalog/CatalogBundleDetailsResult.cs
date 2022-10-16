using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Details about a bundle, from the catalog API.
/// </summary>
/// <remarks>
/// https://catalog.roblox.com/v1/bundles/details?bundleIds=192
/// </remarks>
[DataContract]
internal class CatalogBundleDetailsResult
{
    /// <summary>
    /// The bundle ID.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The bundle type.
    /// </summary>
    [DataMember(Name = "bundleType")]
    public string BundleType { get; set; }

    /// <summary>
    /// The name of the bundle.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The bundle description.
    /// </summary>
    [DataMember(Name = "description")]
    public string Description { get; set; }

    /// <summary>
    /// The creator of the bundle.
    /// </summary>
    [DataMember(Name = "creator")]
    public CatalogItemCreator Creator { get; set; }

    /// <summary>
    /// Bundle product information.
    /// </summary>
    [DataMember(Name = "product")]
    public CatalogBundleProductResult Product { get; set; }

    /// <summary>
    /// The items included in the bundle.
    /// </summary>
    [DataMember(Name = "items")]
    public CatalogBundleItem[] Items { get; set; }

}
