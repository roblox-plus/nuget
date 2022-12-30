using System.Runtime.Serialization;

namespace Roblox.Authentication;

/// <summary>
/// Result of requesting user info via OAuth.
/// </summary>
/// <remarks>
/// https://apis.roblox.com/oauth/v1/userinfo
/// </remarks>
[DataContract]
internal class UserInfoResult
{
    /// <summary>
    /// The Roblox user ID.
    /// </summary>
    [DataMember(Name = "sub")]
    public long Id { get; set; }

    /// <summary>
    /// The Roblox user name.
    /// </summary>
    [DataMember(Name = "preferred_username")]
    public string Name { get; set; }

    /// <summary>
    /// The Roblox user display name.
    /// </summary>
    [DataMember(Name = "name")]
    public string DisplayName { get; set; }
}
