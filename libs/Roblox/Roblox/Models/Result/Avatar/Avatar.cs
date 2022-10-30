using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// Information about a player avatar.
/// </summary>
[DataContract]
public class Avatar
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
    public IReadOnlyCollection<AvatarAsset> Assets { get; set; }
}
