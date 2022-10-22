using System;
using System.Runtime.Serialization;

namespace Roblox.Users;

/// <summary>
/// Configuration for the <see cref="IUsersClient"/>.
/// </summary>
[DataContract(Name = "Users")]
public class UsersClientConfiguration
{
    /// <summary>
    /// How many users to batch together for a single request.
    /// </summary>
    public int BatchSize { get; set; } = 100;

    /// <summary>
    /// The minimum time that must pass between sending batched requests.
    /// </summary>
    public TimeSpan Throttle { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The maximum time to wait before sending a batched request.
    /// </summary>
    public TimeSpan MaxWaitTime { get; set; } = TimeSpan.FromSeconds(15);
}
