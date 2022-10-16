using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Roblox.Api;

namespace Roblox.Users;

/// <inheritdoc cref="IUsersClient"/>
public class UsersClient : IUsersClient
{
    private readonly HttpClient _HttpClient;

    /// <summary>
    /// Initializes a new <seealso cref="UsersClient"/>.
    /// </summary>
    /// <param name="httpClient">The <seealso cref="HttpClient"/> to use to make the requests.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// </exception>
    public UsersClient(HttpClient httpClient)
    {
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc cref="IUsersClient.GetAuthenticatedUserAsync"/>
    public async Task<UserResult> GetAuthenticatedUserAsync(CancellationToken cancellationToken)
    {
        try
        {
            return await _HttpClient.SendApiRequestAsync<UserResult>(HttpMethod.Get, RobloxDomain.UsersApi, $"v1/users/authenticated", queryParameters: null, cancellationToken);
        }
        catch (RobloxUnauthenticatedException)
        {
            return null;
        }
    }

    /// <inheritdoc cref="IUsersClient.GetAllFriendsAsync"/>
    public async Task<IReadOnlyCollection<UserResult>> GetAllFriendsAsync(long userId, CancellationToken cancellationToken)
    {
        var result = await _HttpClient.SendApiRequestAsync<PagedResult<UserResult>>(HttpMethod.Get, RobloxDomain.FriendsApi, $"v1/users/{userId}/friends", queryParameters: null, cancellationToken);
        return result.Data;
    }
}
