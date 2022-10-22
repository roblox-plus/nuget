using System.Runtime.Serialization;
using Roblox.Api;

namespace Roblox.Catalog;

/// <summary>
/// An item returned from a catalog search result.
/// </summary>
[DataContract]
public class CatalogSearchResult
{
    /// <summary>
    /// The ID of the item.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The item type.
    /// </summary>
    /// <seealso cref="RobloxEntity"/>
    [DataMember(Name = "itemType")]
    public string Type { get; set; }
}
