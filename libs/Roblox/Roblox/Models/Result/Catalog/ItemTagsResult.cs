using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace Roblox.Catalog;

[DataContract]
internal class ItemTagsResult
{
    [DataMember(Name = "id")]
    public string Id { get; set; }

    public long? AssetId
    {
        get
        {
            var id = Id.Split(':');
            if (long.TryParse(id.Last(), out var assetId))
            {
                return assetId;
            }

            return null;
        }
    }

    [DataMember(Name = "itemTags")]
    public IReadOnlyCollection<ItemTagResult> Tags { get; set; }
}
