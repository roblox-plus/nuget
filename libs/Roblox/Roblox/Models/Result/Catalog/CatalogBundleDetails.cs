using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Catalog item details, for bundles.
/// </summary>
[DataContract]
public class CatalogBundleDetails
{
    /// <summary>
    /// The bundle ID.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

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
    /// The type of bundle this is.
    /// </summary>
    [DataMember(Name = "bundleType")]
    public string Type { get; set; }

    /// <summary>
    /// The creator of the bundle.
    /// </summary>
    [DataMember(Name = "creator")]
    public CatalogItemCreator Creator { get; set; }

    /// <summary>
    /// Product information for the bundle.
    /// </summary>
    [DataMember(Name = "product")]
    public CatalogItemDetailsProduct Product { get; set; }

    /// <summary>
    /// The items included in the bundle.
    /// </summary>
    [DataMember(Name = "items")]
    public IReadOnlyCollection<CatalogBundleItem> Items { get; set; }
}
