using System;
using System.Runtime.Serialization;

namespace Roblox.Thumbnails;

/// <summary>
/// Thumbnail information.
/// </summary>
[DataContract]
public class ThumbnailResult
{
    /// <summary>
    /// The request identifier.
    /// </summary>
    [DataMember(Name = "requestId")]
    internal string RequestId { get; set; }

    /// <summary>
    /// The thumbnail state.
    /// </summary>
    /// <seealso cref="ThumbnailState"/>
    [DataMember(Name = "state")]
    public string State { get; set; }

    /// <summary>
    /// The URL of the thumbnail.
    /// </summary>
    [DataMember(Name = "imageUrl")]
    public Uri ImageUrl { get; set; }
}
