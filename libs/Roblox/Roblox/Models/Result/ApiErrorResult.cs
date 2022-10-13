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
}
