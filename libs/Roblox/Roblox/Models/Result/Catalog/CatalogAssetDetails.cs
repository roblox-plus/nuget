using System.Runtime.Serialization;
using Roblox.Api;

namespace Roblox.Catalog;

/// <summary>
/// Catalog item details, for assets.
/// </summary>
[DataContract]
public class CatalogAssetDetails
{
    /// <summary>
    /// The asset ID.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The name of the asset.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The asset description.
    /// </summary>
    [DataMember(Name = "description")]
    public string Description { get; set; }

    /// <summary>
    /// The <see cref="AssetType"/>.
    /// </summary>
    [DataMember(Name = "assetType")]
    public AssetType AssetType { get; set; }

    /// <summary>
    /// The creator of the asset.
    /// </summary>
    [DataMember(Name = "creator")]
    public CatalogItemCreator Creator { get; set; }

    /// <summary>
    /// Product information for the asset.
    /// </summary>
    [DataMember(Name = "product")]
    public CatalogItemDetailsProduct Product { get; set; }
}
