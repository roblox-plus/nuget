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
    [DataMember(Name = "id", EmitDefaultValue = false)]
    public long? Id { get; set; }

    /// <summary>
    /// The name of the item associated with the transaction.
    /// </summary>
    [DataMember(Name = "name", EmitDefaultValue = false)]
    public string Name { get; set; }

    /// <summary>
    /// Payments made on the transaction.
    /// </summary>
    [DataMember(Name = "payments", EmitDefaultValue = false)]
    public IReadOnlyCollection<EconomyTransactionPayment> Payments { get; set; }

    /// <summary>
    /// The place associated with the transaction.
    /// </summary>
    [DataMember(Name = "place", EmitDefaultValue = false)]
    public EconomyTransactionPlace Place { get; set; }

    /// <summary>
    /// The type of item the transaction is for.
    /// </summary>
    [DataMember(Name = "type")]
    public string Type { get; set; }
}
