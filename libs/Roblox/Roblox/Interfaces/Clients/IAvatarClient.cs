using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Avatar;

/// <summary>
/// Client for fetching avatar information from Roblox.
/// </summary>
public interface IAvatarClient
{
    /// <summary>
    /// Gets the avatar rules.
    /// </summary>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The avatar rules.</returns>
    Task<AvatarRules> GetAvatarRulesAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets the avatar for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user to load the avatar of.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The avatar.</returns>
    Task<Avatar> GetUserAvatarAsync(long userId, CancellationToken cancellationToken);
}
