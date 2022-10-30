using System.Runtime.Serialization;
using Roblox.Api;

namespace Roblox.Avatar;

/// <summary>
/// An item, as being worn on an avatar.
/// </summary>
[DataContract]
public class AvatarAsset
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
    [DataMember(Name = "type")]
    public AssetType Type { get; set; }

    /// <summary>
    /// The position of the emote in the emote wheel.
    /// </summary>
    [DataMember(Name = "position", EmitDefaultValue = false)]
    public int? EmotePosition { get; set; }
}
