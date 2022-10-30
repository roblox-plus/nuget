using System.Runtime.Serialization;

namespace Roblox.Thumbnails;

[DataContract]
internal class AvatarRenderRequest
{
    [DataMember(Name = "avatarDefinition")]
    public AvatarRenderAvatarConfiguration AvatarConfiguration { get; set; }

    [DataMember(Name = "thumbnailConfig")]
    public AvatarRenderThumbnailConfiguration ThumbnailConfiguration { get; set; }
}
