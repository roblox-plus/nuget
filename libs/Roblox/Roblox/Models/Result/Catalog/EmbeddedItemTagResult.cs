using System.Runtime.Serialization;

namespace Roblox.Catalog;

[DataContract]
internal class EmbeddedItemTagResult
{
    [DataMember(Name = "name")]
    public string Name { get; set; }
}
