using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Authentication;

/// <summary>
/// A client for authenticating with Roblox.
/// </summary>
/// <remarks>
/// For now this is primarily geared towards OAuth 2.0.
/// </remarks>
public interface IAuthenticationClient
{
    /// <summary>
    /// Logs in a user with their authorization code.
    /// </summary>
    /// <remarks>
    /// If the <see cref="OAuthScope.Profile"/> scope is available, it will be used to fetch data for <see cref="LoginResult.User"/>.
    /// If it is unavailable, only the <see cref="UserInfoResult.Id"/> field will be populated.
    /// </remarks>
    /// <param name="code">The authorization code.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The result of logging in.</returns>
    Task<LoginResult> LoginAsync(string code, CancellationToken cancellationToken);

    /// <summary>
    /// Refreshes an authentication session, using the refresh token.
    /// </summary>
    /// <remarks>
    /// If the <see cref="OAuthScope.Profile"/> scope is available, it will be used to fetch data for <see cref="LoginResult.User"/>.
    /// If it is unavailable, only the <see cref="UserInfoResult.Id"/> field will be populated.
    /// </remarks>
    /// <param name="refreshToken">The refresh token.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The result of logging in, with the refreshed session.</returns>
    Task<LoginResult> RefreshAsync(string refreshToken, CancellationToken cancellationToken);

    /// <summary>
    /// Invalidates an access token, or refresh token.
    /// </summary>
    /// <param name="token">The access token, or refresh token to invalidate.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    Task LogoutAsync(string token, CancellationToken cancellationToken);
}
