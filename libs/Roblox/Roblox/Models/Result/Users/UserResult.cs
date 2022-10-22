using System.Runtime.Serialization;

namespace Roblox.Users;

/// <summary>
/// Information about a user.
/// </summary>
[DataContract]
public class UserResult
{
    /// <summary>
    /// The ID of the user.
    /// </summary>
    [DataMember(Name = "id")]
    public long Id { get; set; }

    /// <summary>
    /// The user name.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The display name the user has set.
    /// </summary>
    [DataMember(Name = "displayName")]
    public string DisplayName { get; set; }
}
