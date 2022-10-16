using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Request model for fetching individual catalog item details.
/// </summary>
[DataContract]
internal class CatalogItemDetailsRequestItem
{
    /// <summary>
    /// The ID of the item to fetch the details for.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The type of the item to fetch the details for.
    /// </summary>
    [DataMember(Name = "itemType")]
    public string Type { get; set; }
}
