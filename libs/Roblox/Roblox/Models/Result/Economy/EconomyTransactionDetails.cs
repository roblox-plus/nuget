using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Economy;

/// <summary>
/// Details about an economy transaction.
/// </summary>
[DataContract]
public class EconomyTransactionDetails
{
    /// <summary>
    /// The ID of the item associated with the transaction.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The name of the item associated with the transaction.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Payments made on the transaction.
    /// </summary>
    [DataMember(Name = "payments")]
    public IReadOnlyCollection<EconomyTransactionPayment> Payments { get; set; }

    /// <summary>
    /// The place associated with the transaction.
    /// </summary>
    [DataMember(Name = "place")]
    public EconomyTransactionPlace Place { get; set; }

    /// <summary>
    /// The type of item the transaction is for.
    /// </summary>
    [DataMember(Name = "type")]
    public string Type { get; set; }
}
