using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Users;

/// <summary>
/// A client for fetching user information.
/// </summary>
public interface IUsersClient
{
    /// <summary>
    /// Gets the authenticated user.
    /// </summary>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The <seealso cref="UserResult"/>, or <c>null</c> if the client is unauthenticated.</returns>
    Task<UserResult> GetAuthenticatedUserAsync(CancellationToken cancellationToken);
}
