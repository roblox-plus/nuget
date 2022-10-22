using System;
using System.Runtime.Serialization;

namespace Roblox.Groups;

/// <summary>
/// Configuration for the <see cref="IGroupsClient"/>.
/// </summary>
[DataContract(Name = "Groups")]
public class GroupsClientConfiguration
{
    /// <summary>
    /// How many groups to batch together for a single request.
    /// </summary>
    public int BatchSize { get; set; } = 75;

    /// <summary>
    /// The minimum time that must pass between sending batched requests.
    /// </summary>
    public TimeSpan Throttle { get; set; } = TimeSpan.FromSeconds(5);

    /// <summary>
    /// The maximum time to wait before sending a batched request.
    /// </summary>
    public TimeSpan MaxWaitTime { get; set; } = TimeSpan.FromSeconds(15);
}
