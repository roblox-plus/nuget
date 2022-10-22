using System.Runtime.Serialization;

namespace Roblox.Inventory;

/// <summary>
/// Bundle ownership information.
/// </summary>
[DataContract]
public class BundleOwnershipResult
{
    /// <summary>
    /// The ID of the owned bundle.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }
}
