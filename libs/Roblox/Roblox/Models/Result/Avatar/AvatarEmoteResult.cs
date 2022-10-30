using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// Avatar emote result model.
/// </summary>
[DataContract]
internal class AvatarEmoteResult
{
    /// <summary>
    /// The asset ID for the emote.
    /// </summary>
    [DataMember(Name = "assetId")]
    public long AssetId { get; set; }

    /// <summary>
    /// The name of the emote.
    /// </summary>
    [DataMember(Name = "assetName")]
    public string AssetName { get; set; }

    /// <summary>
    /// The position of the emote in the emote wheel.
    /// </summary>
    [DataMember(Name = "position")]
    public int Position { get; set; }
}
