using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Roblox.Api;
using Roblox.Models.Result.Catalog;

namespace Roblox.Catalog;

/// <inheritdoc cref="ICatalogClient"/>
public class CatalogClient : ICatalogClient
{
    private readonly HttpClient _HttpClient;
    private readonly CatalogClientConfiguration _Settings;
    private readonly IBatchClient<long, CatalogAssetDetails> _AssetsClient;
    private readonly IBatchClient<long, CatalogBundleDetails> _BundlesClient;

    /// <summary>
    /// Initializes a new <seealso cref="CatalogClient"/>.
    /// </summary>
    /// <param name="httpClient">An <see cref="HttpClient"/>.</param>
    /// <param name="configuration">An <see cref="IConfiguration"/>.</param>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="httpClient"/>
    /// - <paramref name="configuration"/>
    /// </exception>
    public CatalogClient(HttpClient httpClient, IConfiguration configuration)
    {
        _HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));

        var settings = _Settings = new CatalogClientConfiguration();
        configuration.BindClientConfiguration(settings);

        if (settings.AssetBatchSize > 1)
        {
            _AssetsClient = new BatchingClient<long, CatalogAssetDetails>(MultiGetAssetsAsync, batchSize: settings.AssetBatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
        }

        if (settings.BundleBatchSize > 1)
        {
            _BundlesClient = new BatchingClient<long, CatalogBundleDetails>(MultiGetBundlesAsync, batchSize: settings.BundleBatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
        }
    }

    /// <inheritdoc cref="ICatalogClient.GetAssetAsync"/>
    public async Task<CatalogAssetDetails> GetAssetAsync(long assetId, CancellationToken cancellationToken)
    {
        var asset = await GetAssetByIdAsync(assetId, cancellationToken);

        if (_Settings.ResaleDataEnabled && asset.Product?.Limited == true)
        {
            var resaleUrl = RobloxDomain.Build(RobloxDomain.EconomyApi, $"v1/assets/{assetId}/resale-data");
            var resaleData = await _HttpClient.SendApiRequestAsync<CatalogAssetResaleDataResult>(HttpMethod.Get, resaleUrl, cancellationToken);
            asset.Product.Price ??= resaleData.OriginalPrice;
            asset.Product.CountRemaining ??= resaleData.NumberRemaining;
            asset.Product.TotalAvailable ??= resaleData.TotalAvailable;
            asset.Product.RecentAveragePrice ??= resaleData.RecentAveragePrice;
        }

        return asset;
    }

    /// <inheritdoc cref="ICatalogClient.GetBundleAsync"/>
    public async Task<CatalogBundleDetails> GetBundleAsync(long bundleId, CancellationToken cancellationToken)
    {
        if (_BundlesClient == null)
        {
            var result = await MultiGetBundlesAsync(new[] { bundleId }, cancellationToken);
            if (result.TryGetValue(bundleId, out var bundle))
            {
                return bundle;
            }

            return null;
        }

        return await _BundlesClient.GetAsync(bundleId, cancellationToken);
    }

    private async Task<CatalogAssetDetails> GetAssetByIdAsync(long assetId, CancellationToken cancellationToken)
    {
        if (_AssetsClient == null)
        {
            var result = await MultiGetAssetsAsync(new[] { assetId }, cancellationToken);
            if (result.TryGetValue(assetId, out var asset))
            {
                return asset;
            }

            return null;
        }

        return await _AssetsClient.GetAsync(assetId, cancellationToken);
    }

    private async Task<IReadOnlyDictionary<long, CatalogAssetDetails>> MultiGetAssetsAsync(IReadOnlyCollection<long> assetIds, CancellationToken cancellationToken)
    {
        var url = RobloxDomain.Build(RobloxDomain.CatalogApi, "v1/catalog/items/details");
        var requestBody = new CatalogItemDetailsRequest
        {
            Data = assetIds.Select(id => new CatalogItemDetailsRequestItem
            {
                Id = id,
                Type = "Asset"
            }).ToArray()
        };

        var pagedResult = await _HttpClient.SendApiRequestAsync<CatalogItemDetailsRequest, PagedResult<CatalogItemDetailsResult>>(HttpMethod.Post, url, requestBody, cancellationToken);
        var assetsById = pagedResult.Data.ToDictionary(d => d.Id, d => d);
        var result = new Dictionary<long, CatalogAssetDetails>();

        foreach (var id in assetIds)
        {
            if (assetsById.TryGetValue(id, out var asset))
            {
                var productDetails = asset.ProductId.HasValue ? new CatalogItemDetailsProduct
                {
                    Id = asset.ProductId.Value,
                    Limited = asset.Restrictions.Contains("Limited") || asset.Restrictions.Contains("LimitedUnique"),
                    Price = asset.Price,
                    PremiumPrice = asset.PremiumPricing?.Price,
                    LowestPrice = asset.LowestPrice,
                    CountRemaining = asset.CountRemaining
                } : null;

                result[id] = new CatalogAssetDetails
                {
                    Id = asset.Id,
                    AssetType = asset.AssetTypeId.HasValue ? (AssetType)asset.AssetTypeId.Value : AssetType.Invalid,
                    Name = asset.Name,
                    Description = asset.Description,
                    Creator = new CatalogItemCreator
                    {
                        Id = asset.CreatorId,
                        Name = asset.CreatorName,
                        Type = asset.CreatorType
                    },
                    Product = productDetails
                };
            }
            else
            {
                result[id] = null;
            }
        }

        return result;
    }

    private async Task<IReadOnlyDictionary<long, CatalogBundleDetails>> MultiGetBundlesAsync(IReadOnlyCollection<long> bundleIds, CancellationToken cancellationToken)
    {
        var url = RobloxDomain.Build(RobloxDomain.CatalogApi, $"v1/bundles/details?bundleIds={string.Join(',', bundleIds)}");
        var pagedResult = await _HttpClient.SendApiRequestAsync<CatalogBundleDetailsResult[]>(HttpMethod.Get, url, cancellationToken);
        var bundlesById = pagedResult.ToDictionary(b => b.Id, b => b);
        var result = new Dictionary<long, CatalogBundleDetails>();

        foreach (var id in bundleIds)
        {
            if (bundlesById.TryGetValue(id, out var bundle))
            {
                var productDetails = bundle.Product != null ? new CatalogItemDetailsProduct
                {
                    Id = bundle.Product.Id,
                    Price = bundle.Product.Free ? 0 : bundle.Product.Price,
                } : null;

                result[id] = new CatalogBundleDetails
                {
                    Id = bundle.Id,
                    Type = bundle.BundleType,
                    Name = bundle.Name,
                    Description = bundle.Description,
                    Creator = bundle.Creator,
                    Product = productDetails,
                    Items = bundle.Items
                };
            }
            else
            {
                result[id] = null;
            }
        }

        return result;
    }
}
