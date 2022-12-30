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
    /// Requires the openid + profile scopes.
    /// </remarks>
    /// <param name="code">The authorization code.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The result of logging in.</returns>
    Task<LoginResult> LoginAsync(string code, CancellationToken cancellationToken);
}
