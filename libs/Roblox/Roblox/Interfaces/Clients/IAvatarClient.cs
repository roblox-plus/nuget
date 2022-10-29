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
}
