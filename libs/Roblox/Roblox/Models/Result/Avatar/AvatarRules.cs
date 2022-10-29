using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// The avatar rules.
/// </summary>
[DataContract]
public class AvatarRules
{
    /// <summary>
    /// Body colors that can be applied to avatars.
    /// </summary>
    [DataMember(Name = "basicBodyColorsPalette")]
    public IReadOnlyCollection<BodyColor> BodyColors { get; set; }
}
