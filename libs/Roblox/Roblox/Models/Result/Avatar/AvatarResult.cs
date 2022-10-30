using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// Information about a player avatar.
/// </summary>
/// <remarks>
/// Used to deserialize the avatar response.
/// </remarks>
[DataContract]
internal class AvatarResult
{
    /// <summary>
    /// The avatar type.
    /// </summary>
    [DataMember(Name = "playerAvatarType")]
    public string Type { get; set; }

    /// <summary>
    /// The avatar body colors.
    /// </summary>
    [DataMember(Name = "bodyColors")]
    public AvatarColors BodyColors { get; set; }

    /// <summary>
    /// The avatar scaling information.
    /// </summary>
    [DataMember(Name = "scales")]
    public AvatarScales Scales { get; set; }

    /// <summary>
    /// The assets the avatar is wearing.
    /// </summary>
    [DataMember(Name = "assets")]
    public IReadOnlyCollection<AvatarAssetResult> Assets { get; set; }

    /// <summary>
    /// The emotes the avatar has equipped.
    /// </summary>
    [DataMember(Name = "emotes")]
    public IReadOnlyCollection<AvatarEmoteResult> Emotes { get; set; }
}
