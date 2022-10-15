using System;
using System.Runtime.Serialization;

namespace Roblox.Inventory;

/// <summary>
/// Information about an individual asset ownership record.
/// </summary>
[DataContract]
public class AssetOwnershipResult
{
    /// <summary>
    /// The ownership record ID.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The serial number associated with the ownership record.
    /// </summary>
    [DataMember(Name = "serialNumber")]
    public int? SerialNumber { get; set; }

    /// <summary>
    /// The current owner for the ownership record.
    /// </summary>
    /// <remarks>
    /// Can be <c>null</c> if the owner has requested their inventory to be private.
    /// </remarks>
    [DataMember(Name = "owner")]
    public AssetOwnerResult Owner { get; set; }

    /// <summary>
    /// When the ownership record was created.
    /// </summary>
    [DataMember(Name = "created")]
    public DateTime Created { get; set; }

    /// <summary>
    /// When the ownership record was last updated.
    /// </summary>
    [DataMember(Name = "updated")]
    public DateTime Updated { get; set; }
}
