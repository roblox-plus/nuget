using System;
using System.Runtime.Serialization;

namespace Roblox.Economy;

/// <summary>
/// Represents information about an economy transaction.
/// </summary>
[DataContract]
public class EconomyTransaction
{
    /// <summary>
    /// The transaction ID.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// Whether or not the transaction is pending.
    /// </summary>
    [DataMember(Name = "isPending")]
    public bool Pending { get; set; }

    /// <summary>
    /// The other party involved in the transaction.
    /// </summary>
    [DataMember(Name = "agent")]
    public EconomyTransactionAgent Agent { get; set; }

    /// <summary>
    /// The amount of currency involved in the transaction.
    /// </summary>
    [DataMember(Name = "currency")]
    public EconomyTransactionCurrency Currency { get; set; }

    /// <summary>
    /// Details about the specific item involved in the transaction.
    /// </summary>
    [DataMember(Name = "details")]
    public EconomyTransactionDetails Details { get; set; }

    /// <summary>
    /// When the transaction occurred.
    /// </summary>
    [DataMember(Name = "created")]
    public DateTime Created { get; set; }
}
