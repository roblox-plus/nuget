using System.Runtime.Serialization;

namespace Roblox.Economy;

/// <summary>
/// Transaction cost details.
/// </summary>
[DataContract]
public class EconomyTransactionCurrency
{
    /// <summary>
    /// The amount paid in the transaction.
    /// </summary>
    [DataMember(Name = "amount")]
    public long Amount { get; set; }

    /// <summary>
    /// The currency type used for the transaction.
    /// </summary>
    /// <remarks>
    /// - Robux
    /// - Tickets
    /// </remarks>
    [DataMember(Name = "type")]
    public string Type { get; set; }
}
