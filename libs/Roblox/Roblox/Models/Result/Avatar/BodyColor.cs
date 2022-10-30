using System.Runtime.Serialization;

namespace Roblox.Avatar;

/// <summary>
/// Information about a body color.
/// </summary>
[DataContract]
public class BodyColor
{
    /// <summary>
    /// The color ID.
    /// </summary>
    [DataMember(Name = "brickColorId")]
    public int Id { get; set; }

    /// <summary>
    /// The name of the color.
    /// </summary>
    [DataMember(Name = "name")]
    public string Name { get; set; }

    /// <summary>
    /// The hex value for the color.
    /// </summary>
    [DataMember(Name = "hexColor")]
    public string Hex { get; set; }
}
