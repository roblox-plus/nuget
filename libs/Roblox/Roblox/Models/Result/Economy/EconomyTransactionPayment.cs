using System;
using System.Runtime.Serialization;

namespace Roblox.Economy;

/// <summary>
/// Payment details for an economy transaction.
/// </summary>
[DataContract]
public class EconomyTransactionPayment
{
    /// <summary>
    /// When the payment occurred.
    /// </summary>
    [DataMember(Name = "created")]
    public DateTime Created { get; set; }
}
