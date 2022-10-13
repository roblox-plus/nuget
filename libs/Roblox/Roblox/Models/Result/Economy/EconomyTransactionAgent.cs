using System.Runtime.Serialization;

namespace Roblox.Economy;

/// <summary>
/// Details of the alternate party in a transaction.
/// </summary>
[DataContract]
public class EconomyTransactionAgent
{
    /// <summary>
    /// The ID of the agent involved in the transaction.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The agent type.
    /// </summary>
    /// <example>
    /// - User
    /// - Group
    /// </example>
    [DataMember(Name = "type")]
    public string Type { get; set; }

    /// <summary>
    /// The name of the alternate party.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }
}
