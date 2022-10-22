using System.Runtime.Serialization;

namespace Roblox.Catalog;

[DataContract]
internal class ItemTagResult
{
    [DataMember(Name = "tag")]
    public EmbeddedItemTagResult Tag { get; set; }
}
