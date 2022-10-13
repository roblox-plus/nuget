using System.Runtime.Serialization;

namespace Roblox.Economy;

/// <summary>
/// Place details on an economy transaction.
/// </summary>
[DataContract]
public class EconomyTransactionPlace
{
    /// <summary>
    /// The ID of the place.
    /// </summary>
    [DataMember(Name = "placeId")]
    public long? Id { get; set; }

    /// <summary>
    /// The ID of the universe the place belongs to.
    /// </summary>
    [DataMember(Name = "universeId")]
    public long? UniverseId { get; set; }

    /// <summary>
    /// The name of the place.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }
}
