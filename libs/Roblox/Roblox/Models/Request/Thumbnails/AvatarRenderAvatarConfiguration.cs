using System.Runtime.Serialization;
using Roblox.Avatar;

namespace Roblox.Thumbnails;

[DataContract]
internal class AvatarRenderAvatarConfiguration
{
    /// <summary>
    /// The body colors for the render.
    /// </summary>
    [DataMember(Name = "bodyColors")]
    public AvatarRenderBodyColors BodyColors { get; set; }

    /// <summary>
    /// The assets to render on the avatar.
    /// </summary>
    [DataMember(Name = "assets")]
    public AvatarAsset[] Assets { get; set; }

    /// <summary>
    /// The avatar scaling information for the render.
    /// </summary>
    [DataMember(Name = "scales")]
    public AvatarScales Scales { get; set; }

    /// <summary>
    /// The type of avatar to render.
    /// </summary>
    [DataMember(Name = "playerAvatarType")]
    public AvatarRenderAvatarType Type { get; set; }
}
