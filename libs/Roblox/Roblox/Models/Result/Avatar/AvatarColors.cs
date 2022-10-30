using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// Avatar colors.
/// </summary>
[DataContract]
public class AvatarColors
{
    /// <summary>
    /// The ID of the color for the avatar's head.
    /// </summary>
    [DataMember(Name = "headColorId")]
    public int HeadColorId { get; set; }

    /// <summary>
    /// The ID of the color for the avatar's torso.
    /// </summary>
    [DataMember(Name = "torsoColorId")]
    public int TorsoColorId { get; set; }

    /// <summary>
    /// The ID of the color for the avatar's right arm.
    /// </summary>
    [DataMember(Name = "rightArmColorId")]
    public int RightArmColorId { get; set; }

    /// <summary>
    /// The ID of the color for the avatar's left arm.
    /// </summary>
    [DataMember(Name = "leftArmColorId")]
    public int LeftArmColorId { get; set; }

    /// <summary>
    /// The ID of the color for the avatar's right leg.
    /// </summary>
    [DataMember(Name = "rightLegColorId")]
    public int RightLegColorId { get; set; }

    /// <summary>
    /// The ID of the color for the avatar's left leg.
    /// </summary>
    [DataMember(Name = "leftLegColorId")]
    public int LeftLegColorId { get; set; }
}
