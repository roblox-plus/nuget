using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Groups;

/// <summary>
/// A client for fetching group information.
/// </summary>
public interface IGroupsClient
{
    /// <summary>
    /// Gets a group by its ID.
    /// </summary>
    /// <param name="id">The group ID.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="GroupResult"/>, or <c>null</c> if the user does not exist.</returns>
    Task<GroupResult> GetGroupByIdAsync(long id, CancellationToken cancellationToken);
}
