using System;
using System.Runtime.Serialization;
using Roblox.Avatar;

namespace Roblox.Thumbnails;

[DataContract]
internal class AvatarRenderBodyColors
{
    [DataMember(Name = "headColor")]
    public string HeadColor { get; set; }

    [DataMember(Name = "torsoColor")]
    public string TorsoColor { get; set; }

    [DataMember(Name = "rightArmColor")]
    public string RightArmColor { get; set; }

    [DataMember(Name = "leftArmColor")]
    public string LeftArmColor { get; set; }

    [DataMember(Name = "rightLegColor")]
    public string RightLegColor { get; set; }

    [DataMember(Name = "leftLegColor")]
    public string LeftLegColor { get; set; }

    public AvatarRenderBodyColors(AvatarRules avatarRules, AvatarColors avatarColors)
    {
        HeadColor = GetHexById(avatarRules, avatarColors.HeadColorId);
        TorsoColor = GetHexById(avatarRules, avatarColors.TorsoColorId);
        RightArmColor = GetHexById(avatarRules, avatarColors.RightArmColorId);
        LeftArmColor = GetHexById(avatarRules, avatarColors.LeftArmColorId);
        RightLegColor = GetHexById(avatarRules, avatarColors.RightLegColorId);
        LeftLegColor = GetHexById(avatarRules, avatarColors.LeftLegColorId);
    }

    private string GetHexById(AvatarRules avatarRules, int bodyColorId)
    {
        foreach (var color in avatarRules.BodyColors)
        {
            if (color.Id == bodyColorId)
            {
                return color.Hex;
            }
        }

        throw new ArgumentException($"{nameof(bodyColorId)} ({bodyColorId}) does not map to known color", nameof(bodyColorId));
    }
}
