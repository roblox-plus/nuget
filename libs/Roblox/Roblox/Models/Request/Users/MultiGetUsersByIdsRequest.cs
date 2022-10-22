using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Users;

[DataContract]
internal class MultiGetUsersByIdsRequest
{
    [DataMember(Name = "userIds")]
    public IReadOnlyCollection<long> Ids { get; set; }

    [DataMember(Name = "excludeBannedUsers")]
    public bool ExcludeBannedUsers { get; set; } = false;
}
