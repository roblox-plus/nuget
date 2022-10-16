using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Request model for fetching catalog item details.
/// </summary>
[DataContract]
internal class CatalogItemDetailsRequest
{
    /// <summary>
    /// The catalog items to fetch details for.
    /// </summary>
    [DataMember(Name = "items")]
    public CatalogItemDetailsRequestItem[] Data { get; set; }
}
