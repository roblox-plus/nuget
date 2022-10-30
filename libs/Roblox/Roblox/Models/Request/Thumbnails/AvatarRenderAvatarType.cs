using System.Runtime.Serialization;

namespace Roblox.Thumbnails;

[DataContract]
internal class AvatarRenderAvatarType
{
    /// <summary>
    /// The avatar type to render.
    /// </summary>
    [DataMember(Name = "playerAvatarType")]
    public string Type { get; set; }
}
