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
    /// T-Shirt
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
    /// Group Emblem
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
    /// Right Arm
    /// </summary>
    [Display(Name = "Right Arm")]
    RightArm = 28,

    /// <summary>
    /// Left Arm
    /// </summary>
    [Display(Name = "Left Arm")]
    LeftArm = 29,

    /// <summary>
    /// Left Leg
    /// </summary>
    [Display(Name = "Left Leg")]
    LeftLeg = 30,

    /// <summary>
    /// Right Leg
    /// </summary>
    [Display(Name = "Right Leg")]
    RightLeg = 31,

    /// <summary>
    /// Package
    /// </summary>
    [Obsolete("Packages were replaced by bundles.")]
    Package = 32,

    /// <summary>
    /// YouTube Video
    /// </summary>
    [Display(Name = "YouTube Video")]
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
    /// Solid Model
    /// </summary>
    [Display(Name = "Solid Model")]
    SolidModel = 39,

    /// <summary>
    /// MeshPart
    /// </summary>
    [Display(Name = "Mesh Part")]
    MeshPart = 40,

    /// <summary>
    /// HairAccessory
    /// </summary>
    [Display(Name = "Hair")]
    HairAccessory = 41,

    /// <summary>
    /// Face Accessory
    /// </summary>
    [Display(Name = "Face Accessory")]
    FaceAccessory = 42,

    /// <summary>
    /// Neck Accessory
    /// </summary>
    [Display(Name = "Neck Accessory")]
    NeckAccessory = 43,

    /// <summary>
    /// Shoulder Accessory
    /// </summary>
    [Display(Name = "Shoulder Accessory")]
    ShoulderAccessory = 44,

    /// <summary>
    /// Front Accessory
    /// </summary>
    [Display(Name = "Front Accessory")]
    FrontAccessory = 45,

    /// <summary>
    /// Back Accessory
    /// </summary>
    [Display(Name = "Back Accessory")]
    BackAccessory = 46,

    /// <summary>
    /// Waist Accessory
    /// </summary>
    [Display(Name = "Waist Accessory")]
    WaistAccessory = 47,

    /// <summary>
    /// Climb Animation
    /// </summary>
    [Display(Name = "Climb Animation")]
    ClimbAnimation = 48,

    /// <summary>
    /// Death Animation
    /// </summary>
    [Display(Name = "Death Animation")]
    DeathAnimation = 49,

    /// <summary>
    /// Fall Animation
    /// </summary>
    [Display(Name = "Fall Animation")]
    FallAnimation = 50,

    /// <summary>
    /// Idle Animation
    /// </summary>
    [Display(Name = "Idle Animation")]
    IdleAnimation = 51,

    /// <summary>
    /// Jump Animation
    /// </summary>
    [Display(Name = "Jump Animation")]
    JumpAnimation = 52,

    /// <summary>
    /// Run Animation
    /// </summary>
    [Display(Name = "Run Animation")]
    RunAnimation = 53,

    /// <summary>
    /// Swim Animation
    /// </summary>
    [Display(Name = "Swim Animation")]
    SwimAnimation = 54,

    /// <summary>
    /// Walk Animation
    /// </summary>
    [Display(Name = "Walk Animation")]
    WalkAnimation = 55,

    /// <summary>
    /// Pose Animation
    /// </summary>
    [Display(Name = "Pose Animation")]
    PoseAnimation = 56,

    /// <summary>
    /// Ear Accessory
    /// </summary>
    [Display(Name = "Ear Accessory")]
    [Obsolete("None of these exist on Roblox, and it appears to be obsolete.")]
    EarAccessory = 57,

    /// <summary>
    /// Eye Accessory
    /// </summary>
    [Display(Name = "Eye Accessory")]
    [Obsolete("None of these exist on Roblox, and it appears to be obsolete.")]
    EyeAccessory = 58,

    /// <summary>
    /// Localization Table Manifest
    /// </summary>
    [Display(Name = "Localization Table Manifest")]
    LocalizationTableManifest = 59,

    /// <summary>
    /// Localization Table Translation
    /// </summary>
    [Display(Name = "Localization Table Translation")]
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
    /// Texture Pack
    /// </summary>
    [Display(Name = "Texture Pack")]
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
    /// Font Family
    /// </summary>
    [Display(Name = "Font Family")]
    FontFamily = 73,

    /// <summary>
    /// Font Face
    /// </summary>
    [Display(Name = "Font Face")]
    FontFace = 74,

    /// <summary>
    /// Mesh-Hidden Surface Removal
    /// </summary>
    [Display(Name = "Mesh-Hidden Surface Removal")]
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
    [Display(Name = "Mood Animation")]
    MoodAnimation = 78,

    /// <summary>
    /// DynamicHead
    /// </summary>
    [Display(Name = "Head")]
    DynamicHead = 79,
}
