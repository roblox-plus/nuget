using System.Runtime.Serialization;

namespace Roblox.Thumbnails;

[DataContract]
internal class AvatarRenderThumbnailConfiguration
{
    [DataMember(Name = "size")]
    public string Size { get; set; }

    [DataMember(Name = "thumbnailId")]
    public long ThumbnailId { get; } = 2;

    [DataMember(Name = "thumbnailTYpe")]
    public string Type { get; } = "2d";
}
