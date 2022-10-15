using System.Runtime.Serialization;
using Roblox.Api;

namespace Roblox.Inventory;

/// <summary>
/// The owner of an <seealso cref="AssetOwnershipResult"/>.
/// </summary>
[DataContract]
public class AssetOwnerResult
{
    /// <summary>
    /// The ID of the owner.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The name of the owner.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The owner type.
    /// </summary>
    /// <seealso cref="RobloxEntity"/>
    [DataMember(Name = "type")]
    public string Type { get; set; }
}
