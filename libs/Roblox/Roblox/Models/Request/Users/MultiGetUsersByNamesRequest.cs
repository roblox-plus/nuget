using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Users;

[DataContract]
internal class MultiGetUsersByNamesRequest
{
    [DataMember(Name = "usernames")]
    public IReadOnlyCollection<string> Names { get; set; }

    [DataMember(Name = "excludeBannedUsers")]
    public bool ExcludeBannedUsers { get; set; } = false;
}
