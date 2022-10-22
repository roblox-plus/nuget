using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Roblox.Api;

namespace Roblox.Groups;

/// <inheritdoc cref="IGroupsClient"/>
public class GroupsClient : IGroupsClient
{
    private readonly HttpClient _HttpClient;
    private readonly IBatchClient<long, GroupResult> _GroupsClient;

    /// <summary>
    /// Initializes a new <seealso cref="GroupsClient"/>.
    /// </summary>
    /// <param name="httpClient">The <seealso cref="HttpClient"/> to use to make the requests.</param>
    /// <param name="configuration">An <see cref="IConfiguration"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// - <paramref name="configuration"/>
    /// </exception>
    public GroupsClient(HttpClient httpClient, IConfiguration configuration)
    {
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        var settings = new GroupsClientConfiguration();
        var clientName = configuration.BindClientConfiguration(settings);

        _GroupsClient = new BatchingClient<long, GroupResult>(MultiGetGroupsByIdsAsync, clientName, batchSize: settings.BatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
    }

    /// <inheritdoc cref="IGroupsClient.GetGroupByIdAsync"/>
    public Task<GroupResult> GetGroupByIdAsync(long id, CancellationToken cancellationToken)
    {
        return _GroupsClient.GetAsync(id, cancellationToken);
    }

    private async Task<IReadOnlyDictionary<long, GroupResult>> MultiGetGroupsByIdsAsync(IReadOnlyCollection<long> ids, CancellationToken cancellationToken)
    {
        var pagedResult = await _HttpClient.SendApiRequestAsync<PagedResult<GroupResult>>(HttpMethod.Get, RobloxDomain.GroupsApi, $"v2/groups", queryParameters: new Dictionary<string, string>
        {
            ["groupIds"] = string.Join(',', ids)
        }, cancellationToken);
        var result = pagedResult.Data.ToDictionary(g => g.Id, g => g);

        foreach (var id in ids)
        {
            if (!result.ContainsKey(id))
            {
                result[id] = null;
            }
        }

        return result;
    }
}
