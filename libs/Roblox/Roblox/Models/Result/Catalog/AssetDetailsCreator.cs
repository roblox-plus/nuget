using System.Runtime.Serialization;

namespace Roblox.Catalog;

[DataContract]
internal class AssetDetailsCreator
{
    [DataMember(Name = "CreatorTargetId")]
    public long Id { get; set; }

    [DataMember(Name = "Name")]
    public string Name { get; set; }

    [DataMember(Name = "CreatorType")]
    public string Type { get; set; }
}
