using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// Avatar scaling information.
/// </summary>
[DataContract]
public class AvatarScales
{
    /// <summary>
    /// The body type.
    /// </summary>
    /// <remarks>
    /// Not quite sure what this is, or if it's supposed to be a double.
    /// </remarks>
    [DataMember(Name = "bodyType")]
    public double BodyType { get; set; }

    /// <summary>
    /// The avatar height.
    /// </summary>
    [DataMember(Name = "height")]
    public double Height { get; set; }

    /// <summary>
    /// The avatar width.
    /// </summary>
    [DataMember(Name = "width")]
    public double Width { get; set; }

    /// <summary>
    /// The depth of the avatar.
    /// </summary>
    [DataMember(Name = "depth")]
    public double Depth { get; set; }

    /// <summary>
    /// The avatar proportions.
    /// </summary>
    [DataMember(Name = "proportion")]
    public double Proportion { get; set; }

    /// <summary>
    /// The scale of the head of the avatar.
    /// </summary>
    [DataMember(Name = "head")]
    public double Head { get; set; }
}
