using System;
using System.ComponentModel.DataAnnotations;

namespace Roblox.Api;

/// <summary>
/// Asset types.
/// </summary>
public enum AssetType
{
    /// <summary>
    /// Exists to catch <c>default</c>.
    /// </summary>
    Invalid = 0,

    /// <summary>
    /// Image
    /// </summary>
    Image = 1,

    /// <summary>
    /// TShirt
    /// </summary>
    [Display(Name = "T-Shirt")]
    TShirt = 2,

    /// <summary>
    /// Audio
    /// </summary>
    Audio = 3,

    /// <summary>
    /// Mesh
    /// </summary>
    Mesh = 4,

    /// <summary>
    /// Lua
    /// </summary>
    Lua = 5,

    /// <summary>
    /// Html
    /// </summary>
    Html = 6,

    /// <summary>
    /// Text
    /// </summary>
    Text = 7,

    /// <summary>
    /// Hat
    /// </summary>
    Hat = 8,

    /// <summary>
    /// Place
    /// </summary>
    Place = 9,

    /// <summary>
    /// Model
    /// </summary>
    Model = 10,

    /// <summary>
    /// Shirt
    /// </summary>
    Shirt = 11,

    /// <summary>
    /// Pants
    /// </summary>
    Pants = 12,

    /// <summary>
    /// Decal
    /// </summary>
    Decal = 13,

    /// <summary>
    /// Avatar
    /// </summary>
    Avatar = 16,

    /// <summary>
    /// Head
    /// </summary>
    Head = 17,

    /// <summary>
    /// Face
    /// </summary>
    Face = 18,

    /// <summary>
    /// Gear
    /// </summary>
    Gear = 19,

    /// <summary>
    /// Badge
    /// </summary>
    [Obsolete("Badges are no longer assets.")]
    Badge = 21,

    /// <summary>
    /// GroupEmblem
    /// </summary>
    [Display(Name = "Group Icon")]
    GroupEmblem = 22,

    /// <summary>
    /// Animation
    /// </summary>
    Animation = 24,

    /// <summary>
    /// Arms
    /// </summary>
    Arms = 25,

    /// <summary>
    /// Legs
    /// </summary>
    Legs = 26,

    /// <summary>
    /// Torso
    /// </summary>
    Torso = 27,

    /// <summary>
    /// RightArm
    /// </summary>
    RightArm = 28,

    /// <summary>
    /// LeftArm
    /// </summary>
    LeftArm = 29,

    /// <summary>
    /// LeftLeg
    /// </summary>
    LeftLeg = 30,

    /// <summary>
    /// RightLeg
    /// </summary>
    RightLeg = 31,

    /// <summary>
    /// Package
    /// </summary>
    [Obsolete("Packages were replaced by bundles.")]
    Package = 32,

    /// <summary>
    /// YouTubeVideo
    /// </summary>
    YouTubeVideo = 33,

    /// <summary>
    /// GamePass
    /// </summary>
    [Obsolete("Game passes are no longer assets.")]
    GamePass = 34,

    /// <summary>
    /// App
    /// </summary>
    App = 35,

    /// <summary>
    /// Code
    /// </summary>
    Code = 37,

    /// <summary>
    /// Plugin
    /// </summary>
    Plugin = 38,

    /// <summary>
    /// SolidModel
    /// </summary>
    SolidModel = 39,

    /// <summary>
    /// MeshPart
    /// </summary>
    MeshPart = 40,

    /// <summary>
    /// HairAccessory
    /// </summary>
    [Display(Name = "Hair")]
    HairAccessory = 41,

    /// <summary>
    /// FaceAccessory
    /// </summary>
    [Display(Name = "Face Accessory")]
    FaceAccessory = 42,

    /// <summary>
    /// NeckAccessory
    /// </summary>
    [Display(Name = "Neck Accessory")]
    NeckAccessory = 43,

    /// <summary>
    /// ShoulderAccessory
    /// </summary>
    [Display(Name = "Shoulder Accessory")]
    ShoulderAccessory = 44,

    /// <summary>
    /// FrontAccessory
    /// </summary>
    [Display(Name = "Front Accessory")]
    FrontAccessory = 45,

    /// <summary>
    /// BackAccessory
    /// </summary>
    [Display(Name = "Back Accessory")]
    BackAccessory = 46,

    /// <summary>
    /// WaistAccessory
    /// </summary>
    [Display(Name = "Waist Accessory")]
    WaistAccessory = 47,

    /// <summary>
    /// ClimbAnimation
    /// </summary>
    ClimbAnimation = 48,

    /// <summary>
    /// DeathAnimation
    /// </summary>
    DeathAnimation = 49,

    /// <summary>
    /// FallAnimation
    /// </summary>
    FallAnimation = 50,

    /// <summary>
    /// IdleAnimation
    /// </summary>
    IdleAnimation = 51,

    /// <summary>
    /// JumpAnimation
    /// </summary>
    JumpAnimation = 52,

    /// <summary>
    /// RunAnimation
    /// </summary>
    RunAnimation = 53,

    /// <summary>
    /// SwimAnimation
    /// </summary>
    SwimAnimation = 54,

    /// <summary>
    /// WalkAnimation
    /// </summary>
    WalkAnimation = 55,

    /// <summary>
    /// PoseAnimation
    /// </summary>
    PoseAnimation = 56,

    /// <summary>
    /// EarAccessory
    /// </summary>
    [Obsolete("None of these exist on Roblox, and it appears to be obsolete.")]
    EarAccessory = 57,

    /// <summary>
    /// EyeAccessory
    /// </summary>
    [Obsolete("None of these exist on Roblox, and it appears to be obsolete.")]
    EyeAccessory = 58,

    /// <summary>
    /// LocalizationTableManifest
    /// </summary>
    LocalizationTableManifest = 59,

    /// <summary>
    /// LocalizationTableTranslation
    /// </summary>
    LocalizationTableTranslation = 60,

    /// <summary>
    /// Emote
    /// </summary>
    Emote = 61,

    /// <summary>
    /// Video
    /// </summary>
    Video = 62,

    /// <summary>
    /// TexturePack
    /// </summary>
    TexturePack = 63,

    /// <summary>
    /// TShirtAccessory
    /// </summary>
    [Display(Name = "T-Shirt")]
    TShirtAccessory = 64,

    /// <summary>
    /// ShirtAccessory
    /// </summary>
    [Display(Name = "Shirt")]
    ShirtAccessory = 65,

    /// <summary>
    /// PantsAccessory
    /// </summary>
    [Display(Name = "Pants")]
    PantsAccessory = 66,

    /// <summary>
    /// JacketAccessory
    /// </summary>
    [Display(Name = "Jacket")]
    JacketAccessory = 67,

    /// <summary>
    /// SweaterAccessory
    /// </summary>
    [Display(Name = "Sweater")]
    SweaterAccessory = 68,

    /// <summary>
    /// ShortsAccessory
    /// </summary>
    [Display(Name = "Shorts")]
    ShortsAccessory = 69,

    /// <summary>
    /// LeftShoeAccessory
    /// </summary>
    [Display(Name = "Left Shoe")]
    LeftShoeAccessory = 70,

    /// <summary>
    /// RightShoeAccessory
    /// </summary>
    [Display(Name = "Right Shoe")]
    RightShoeAccessory = 71,

    /// <summary>
    /// DressSkirtAccessory
    /// </summary>
    [Display(Name = "Skirt")]
    DressSkirtAccessory = 72,

    /// <summary>
    /// FontFamily
    /// </summary>
    FontFamily = 73,

    /// <summary>
    /// FontFace
    /// </summary>
    FontFace = 74,

    /// <summary>
    /// MeshHiddenSurfaceRemoval
    /// </summary>
    MeshHiddenSurfaceRemoval = 75,

    /// <summary>
    /// EyebrowAccessory
    /// </summary>
    [Display(Name = "Eyebrows")]
    EyebrowAccessory = 76,

    /// <summary>
    /// EyelashAccessory
    /// </summary>
    [Display(Name = "Eyelashes")]
    EyelashAccessory = 77,

    /// <summary>
    /// MoodAnimation
    /// </summary>
    MoodAnimation = 78,

    /// <summary>
    /// DynamicHead
    /// </summary>
    [Display(Name = "Head")]
    DynamicHead = 79,
}
