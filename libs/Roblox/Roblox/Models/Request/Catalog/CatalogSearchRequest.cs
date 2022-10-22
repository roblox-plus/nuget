using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// Search parameters for the catalog.
/// </summary>
[DataContract]
public class CatalogSearchRequest
{
    /// <summary>
    /// The category parameter.
    /// </summary>
    [DataMember(Name = "category")]
    public string Category { get; set; }

    /// <summary>
    /// The subcategory parameter.
    /// </summary>
    [DataMember(Name = "subcategory")]
    public string Subcategory { get; set; }

    /// <summary>
    /// The sorting for the catalog results.
    /// </summary>
    [DataMember(Name = "sortType")]
    public string SortType { get; set; }

    /// <summary>
    /// The search keyword.
    /// </summary>
    [DataMember(Name = "keyword")]
    public string Keyword { get; set; }

    /// <summary>
    /// The name of the creator to filter the results to.
    /// </summary>
    [DataMember(Name = "creatorName")]
    public string CreatorName { get; set; }

    /// <summary>
    /// Whether or not to include items that are off sale.
    /// </summary>
    [DataMember(Name = "includeNotForSale")]
    public bool IncludeOffSaleItems { get; set; } = true;
}
