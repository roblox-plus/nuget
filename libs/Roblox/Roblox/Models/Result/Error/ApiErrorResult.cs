using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Api;

/// <summary>
/// An API error result.
/// </summary>
[DataContract]
public class ApiErrorResult
{
    /// <summary>
    /// The errors.
    /// </summary>
    [DataMember(Name = "errors")]
    public IReadOnlyCollection<ApiErrorCodeResult> Errors { get; set; }

    /// <summary>
    /// The error code.
    /// </summary>
    /// <remarks>
    /// The OAuth endpoints don't have an errors array, they have two properties: error + error_description.
    /// </remarks>
    [DataMember(Name = "error")]
    public string SingularErrorCode { get; set; }

    /// <summary>
    /// The error message.
    /// </summary>
    [DataMember(Name = "error_description")]
    public string SingularErrorMessage { get; set; }
}
