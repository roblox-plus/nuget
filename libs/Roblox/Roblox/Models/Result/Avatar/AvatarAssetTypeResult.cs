using System.Runtime.Serialization;
using Roblox.Api;

namespace Roblox.Avatar;

/// <summary>
/// The asset type container.
/// </summary>
[DataContract]
internal class AvatarAssetTypeResult
{
    /// <summary>
    /// The actual asset type.
    /// </summary>
    [DataMember(Name = "id")]
    public AssetType Value { get; set; }
}
