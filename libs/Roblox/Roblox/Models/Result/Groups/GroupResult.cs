using System.Runtime.Serialization;

namespace Roblox.Groups;

/// <summary>
/// Information about a group.
/// </summary>
[DataContract]
public class GroupResult
{
    /// <summary>
    /// The ID of the group.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The name of the group.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The owner of the group.
    /// </summary>
    [DataMember(Name = "owner")]
    public GroupOwner Owner { get; set; }
}
