using System.Runtime.Serialization;
using Roblox.Api;

namespace Roblox.Catalog;

[DataContract]
internal class AssetDetailsResult
{
    [DataMember(Name = "TargetId")]
    public long TargetId { get; set; }

    [DataMember(Name = "ProductType")]
    public string ProductType { get; set; }

    [DataMember(Name = "AssetId")]
    public long AssetId { get; set; }

    [DataMember(Name = "ProductId")]
    public long ProductId { get; set; }

    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "Description")]
    public string Description { get; set; }

    [DataMember(Name = "AssetTypeId")]
    public AssetType AssetType { get; set; }

    [DataMember(Name = "Creator")]
    public AssetDetailsCreator Creator { get; set; }

    [DataMember(Name = "PriceInRobux")]
    public long? Price { get; set; }

    [DataMember(Name = "IsForSale")]
    public bool IsForSale { get; set; }

    [DataMember(Name = "IsPublicDomain")]
    public bool Free { get; set; }

    [DataMember(Name = "IsLimited")]
    public bool IsLimited { get; set; }

    [DataMember(Name = "IsLimitedUnique")]
    public bool IsLimitedUnique { get; set; }

    [DataMember(Name = "Remaining")]
    public int? RemainingCount { get; set; }
}
