using System.Runtime.Serialization;

namespace Roblox.Catalog;

/// <summary>
/// An item that is part of a bundle.
/// </summary>
[DataContract]
public class CatalogBundleItem
{
    /// <summary>
    /// The ID of the creator.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The creator name.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The creator type.
    /// </summary>
    [DataMember(Name = "type")]
    public string Type { get; set; }
}
