using System.Collections.Generic;
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
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="UserResult"/>, or <c>null</c> if the client is unauthenticated.</returns>
    Task<UserResult> GetAuthenticatedUserAsync(CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user by their ID.
    /// </summary>
    /// <param name="id">The user ID.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="UserResult"/>, or <c>null</c> if the user does not exist.</returns>
    Task<UserResult> GetUserByIdAsync(long id, CancellationToken cancellationToken);

    /// <summary>
    /// Gets a user by their name.
    /// </summary>
    /// <param name="name">The user name.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="UserResult"/>, or <c>null</c> if the user does not exist.</returns>
    Task<UserResult> GetUserByNameAsync(string name, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all the friends for a user.
    /// </summary>
    /// <remarks>
    /// The information Roblox returns for this API is pretty detailed, but we're returning
    /// minimal information, to reduce chance of introducing breaking changes, if the endpoint
    /// starts returning information, or future versions do not include the details.
    ///
    /// Future proofing.
    /// </remarks>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>All the friends the user has.</returns>
    Task<IReadOnlyCollection<UserResult>> GetAllFriendsAsync(long userId, CancellationToken cancellationToken);
}
