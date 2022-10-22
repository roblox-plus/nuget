using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Roblox.Api;

namespace Roblox.Users;

/// <inheritdoc cref="IUsersClient"/>
public class UsersClient : IUsersClient
{
    private readonly HttpClient _HttpClient;
    private readonly IBatchClient<long, UserResult> _UserIdsClient;
    private readonly IBatchClient<string, UserResult> _UsernamesClient;

    /// <summary>
    /// Initializes a new <seealso cref="UsersClient"/>.
    /// </summary>
    /// <param name="httpClient">The <seealso cref="HttpClient"/> to use to make the requests.</param>
    /// <param name="configuration">An <see cref="IConfiguration"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// - <paramref name="configuration"/>
    /// </exception>
    public UsersClient(HttpClient httpClient, IConfiguration configuration)
    {
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        var settings = new UsersClientConfiguration();
        var clientName = configuration.BindClientConfiguration(settings);

        _UserIdsClient = new BatchingClient<long, UserResult>(MultiGetUsersByIdsAsync, clientName, batchSize: settings.BatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
        _UsernamesClient = new BatchingClient<string, UserResult>(MultiGetUsersByNamesAsync, clientName, batchSize: settings.BatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
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

    /// <inheritdoc cref="IUsersClient.GetUserByIdAsync"/>
    public Task<UserResult> GetUserByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _UserIdsClient.GetAsync(id, cancellationToken);
    }

    /// <inheritdoc cref="IUsersClient.GetUserByNameAsync"/>
    public Task<UserResult> GetUserByNameAsync(string name, CancellationToken cancellationToken)
    {
        return _UsernamesClient.GetAsync(name, cancellationToken);
    }

    /// <inheritdoc cref="IUsersClient.GetAllFriendsAsync"/>
    public async Task<IReadOnlyCollection<UserResult>> GetAllFriendsAsync(long userId, CancellationToken cancellationToken)
    {
        var result = await _HttpClient.SendApiRequestAsync<PagedResult<UserResult>>(HttpMethod.Get, RobloxDomain.FriendsApi, $"v1/users/{userId}/friends", queryParameters: null, cancellationToken);
        return result.Data;
    }

    private async Task<IReadOnlyDictionary<long, UserResult>> MultiGetUsersByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken)
    {
        var requestBody = new MultiGetUsersByIdsRequest
        {
            Ids = ids
        };

        var pagedResult = await _HttpClient.SendApiRequestAsync<MultiGetUsersByIdsRequest, PagedResult<UserResult>>(HttpMethod.Post, RobloxDomain.UsersApi, $"v1/users", queryParameters: null, requestBody, cancellationToken);
        var result = pagedResult.Data.ToDictionary(u => u.Id, u => u);

        foreach (var id in ids)
        {
            if (!result.ContainsKey(id))
            {
                result[id] = null;
            }
        }

        return result;
    }

    private async Task<IReadOnlyDictionary<string, UserResult>> MultiGetUsersByNamesAsync(IReadOnlyCollection<string> names, CancellationToken cancellationToken)
    {
        var requestBody = new MultiGetUsersByNamesRequest
        {
            Names = names
        };

        var pagedResult = await _HttpClient.SendApiRequestAsync<MultiGetUsersByNamesRequest, PagedResult<UserResult>>(HttpMethod.Post, RobloxDomain.UsersApi, $"v1/usernames/users", queryParameters: null, requestBody, cancellationToken);
        var result = pagedResult.Data.ToDictionary(u => u.RequestedUsername, u => u);

        foreach (var name in names)
        {
            if (!result.ContainsKey(name))
            {
                result[name] = null;
            }
        }

        return result;
    }
}
