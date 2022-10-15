using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Roblox.Api;

namespace Roblox.Economy;

/// <inheritdoc cref="IEconomyTransactionsClient"/>
public class EconomyTransactionsClient : IEconomyTransactionsClient
{
    private readonly HttpClient _HttpClient;

    /// <summary>
    /// Initializes a new <seealso cref="EconomyTransactionsClient"/>.
    /// </summary>
    /// <param name="httpClient">The <seealso cref="HttpClient"/> to use to make the requests.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// </exception>
    public EconomyTransactionsClient(HttpClient httpClient)
    {
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc cref="IEconomyTransactionsClient.GetUserTransactionsAsync"/>
    public Task<PagedResult<EconomyTransaction>> GetUserTransactionsAsync(long userId, string transactionType, string cursor, CancellationToken cancellationToken)
    {
        var url = RobloxDomain.Build(RobloxDomain.EconomyApi, $"v2/users/{userId}/transactions", new Dictionary<string, string>
        {
            ["limit"] = "100",
            ["cursor"] = cursor,
            ["transactionType"] = transactionType
        });

        return _HttpClient.SendApiRequestAsync<PagedResult<EconomyTransaction>>(HttpMethod.Get, url, cancellationToken);
    }

    /// <inheritdoc cref="IEconomyTransactionsClient.GetGroupTransactionsAsync"/>
    public Task<PagedResult<EconomyTransaction>> GetGroupTransactionsAsync(long groupId, string transactionType, string cursor, CancellationToken cancellationToken)
    {
        var url = RobloxDomain.Build(RobloxDomain.EconomyApi, $"v2/groups/{groupId}/transactions", new Dictionary<string, string>
        {
            ["limit"] = "100",
            ["cursor"] = cursor,
            ["transactionType"] = transactionType
        });

        return _HttpClient.SendApiRequestAsync<PagedResult<EconomyTransaction>>(HttpMethod.Get, url, cancellationToken);
    }
}
