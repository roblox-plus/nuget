using System.Runtime.Serialization;

namespace Roblox.Api;

/// <summary>
/// An individual error.
/// </summary>
[DataContract]
public class ApiErrorCodeResult
{
    /// <summary>
    /// The error code.
    /// </summary>
    [DataMember(Name = "code")]
    public int Code { get; set; }

    /// <summary>
    /// The error message.
    /// </summary>
    [DataMember(Name = "message")]
    public string Message { get; set; }
}
