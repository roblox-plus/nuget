using System.Runtime.Serialization;

namespace Roblox.Groups;

/// <summary>
/// Information about a group owner.
/// </summary>
[DataContract]
public class GroupOwner
{
    /// <summary>
    /// The ID of the owner of the group.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The owner type.
    /// </summary>
    [DataMember(Name = "type")]
    public string Type { get; set; }
}
