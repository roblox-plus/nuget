using System;
using System.ComponentModel;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Roblox.Api;

namespace Roblox.Inventory;

/// <inheritdoc cref="IInventoryClient"/>
public class InventoryClient : IInventoryClient
{
    private readonly HttpClient _HttpClient;

    /// <summary>
    /// Initializes a new <seealso cref="InventoryClient"/>.
    /// </summary>
    /// <param name="httpClient">The <seealso cref="HttpClient"/> to use to make the requests.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// </exception>
    public InventoryClient(HttpClient httpClient)
    {
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
    }

    /// <inheritdoc cref="IInventoryClient.GetAssetOwnersAsync"/>
    public Task<PagedResult<AssetOwnershipResult>> GetAssetOwnersAsync(long assetId, string cursor, ListSortDirection sortOrder, CancellationToken cancellationToken)
    {
        return _HttpClient.SendApiRequestAsync<PagedResult<AssetOwnershipResult>>(HttpMethod.Get, RobloxDomain.InventoryApi, $"v2/assets/{assetId}/owners", queryParameters: null, cancellationToken);
    }

    /// <inheritdoc cref="IInventoryClient.GetOwnedBundlesByUserIdAsync"/>
    public Task<PagedResult<BundleOwnershipResult>> GetOwnedBundlesByUserIdAsync(long userId, string cursor, ListSortDirection sortOrder, CancellationToken cancellationToken)
    {
        return _HttpClient.SendApiRequestAsync<PagedResult<BundleOwnershipResult>>(HttpMethod.Get, RobloxDomain.CatalogApi, $"v1/users/{userId}/bundles", queryParameters: cursor.ToPagingParameters(sortOrder), cancellationToken);
    }
}
