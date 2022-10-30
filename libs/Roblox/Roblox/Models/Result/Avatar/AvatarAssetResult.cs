using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// An asset an avatar is wearing.
/// </summary>
[DataContract]
internal class AvatarAssetResult
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
    /// The asset type.
    /// </summary>
    [DataMember(Name = "assetType")]
    public AvatarAssetTypeResult AssetType { get; set; }
}
