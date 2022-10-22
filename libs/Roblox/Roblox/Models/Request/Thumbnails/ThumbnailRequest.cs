using System.Linq;
using System.Runtime.Serialization;

namespace Roblox.Thumbnails;

[DataContract]
internal class ThumbnailRequest
{
    [DataMember(Name = "requestId")]
    public string RequestId { get; set; }

    [DataMember(Name = "targetId")]
    public long TargetId
    {
        get
        {
            return long.Parse(RequestId.Split(':').Last());
        }
    }

    [DataMember(Name = "type")]
    public string Type
    {
        get
        {
            return RequestId.Split(':').First();
        }
    }

    [DataMember(Name = "size")]
    public string Size { get; set; }

    [DataMember(Name = "isCircular")]
    public bool IsCircular { get; set; } = false;

    [DataMember(Name = "format")]
    public string Format { get; set; } = "png";
}
