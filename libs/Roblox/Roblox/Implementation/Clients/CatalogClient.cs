using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    private readonly IBatchClient<long, IReadOnlyCollection<string>> _AssetTagsClient;

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
        var clientName = configuration.BindClientConfiguration(settings);

        _AssetsClient = new BatchingClient<long, CatalogAssetDetails>(MultiGetAssetsAsync, clientName, batchSize: settings.AssetBatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
        _BundlesClient = new BatchingClient<long, CatalogBundleDetails>(MultiGetBundlesAsync, clientName, batchSize: settings.BundleBatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
        _AssetTagsClient = new BatchingClient<long, IReadOnlyCollection<string>>(MultiGetAssetTagsAsync, clientName, batchSize: settings.AssetTagBatchSize, throttle: settings.Throttle, sendInterval: settings.MaxWaitTime);
    }

    /// <inheritdoc cref="ICatalogClient.GetAssetAsync"/>
    public async Task<CatalogAssetDetails> GetAssetAsync(long assetId, CancellationToken cancellationToken)
    {
        var asset = await _AssetsClient.GetAsync(assetId, cancellationToken);

        if (asset == null)
        {
            try
            {
                var assetDetails = await _HttpClient.SendApiRequestAsync<AssetDetailsResult>(HttpMethod.Get, RobloxDomain.EconomyApi, $"v2/assets/{assetId}/details", queryParameters: null, cancellationToken);
                if (assetDetails.TargetId == assetId)
                {
                    var product = assetDetails.ProductId > 0 ? new CatalogItemDetailsProduct
                    {
                        Id = assetDetails.ProductId,
                        Limited = assetDetails.IsLimited || assetDetails.IsLimitedUnique,
                        CountRemaining = assetDetails.RemainingCount,
                        Price = assetDetails.Free ? 0 : (assetDetails.IsForSale ? assetDetails.Price : null)
                    } : null;

                    asset = new CatalogAssetDetails
                    {
                        Id = assetId,
                        Name = assetDetails.Name,
                        Description = assetDetails.Description,
                        AssetType = assetDetails.AssetType,
                        Creator = new CatalogItemCreator
                        {
                            Id = assetDetails.Creator.Id,
                            Name = assetDetails.Creator.Name,
                            Type = assetDetails.Creator.Type
                        },
                        Product = product
                    };
                }
            }
            catch (RobloxApiException ex) when (ex.StatusCode == HttpStatusCode.BadRequest)
            {
                return null;
            }
        }

        if (_Settings.ResaleDataEnabled && asset?.Product?.Limited == true)
        {
            var resaleData = await _HttpClient.SendApiRequestAsync<CatalogAssetResaleDataResult>(HttpMethod.Get, RobloxDomain.EconomyApi, $"v1/assets/{assetId}/resale-data", queryParameters: null, cancellationToken);
            asset.Product.Price ??= resaleData.OriginalPrice;
            asset.Product.CountRemaining ??= resaleData.NumberRemaining;
            asset.Product.TotalAvailable ??= resaleData.TotalAvailable;
            asset.Product.RecentAveragePrice ??= resaleData.RecentAveragePrice;
        }

        return asset;
    }

    /// <inheritdoc cref="ICatalogClient.GetBundleAsync"/>
    public Task<CatalogBundleDetails> GetBundleAsync(long bundleId, CancellationToken cancellationToken)
    {
        return _BundlesClient.GetAsync(bundleId, cancellationToken);
    }

    /// <inheritdoc cref="ICatalogClient.GetBundlesByAssetIdAsync"/>
    public async Task<PagedResult<CatalogBundleDetails>> GetBundlesByAssetIdAsync(long assetId, string cursor, CancellationToken cancellationToken)
    {
        var pagedResult = await _HttpClient.SendApiRequestAsync<PagedResult<CatalogBundleDetailsResult>>(HttpMethod.Get, RobloxDomain.CatalogApi, $"v1/assets/{assetId}/bundles", queryParameters: null, cancellationToken);
        return new PagedResult<CatalogBundleDetails>
        {
            NextPageCursor = pagedResult.NextPageCursor,
            PreviousNextCursor = pagedResult.PreviousNextCursor,
            Data = pagedResult.Data.Select(TranslateBundle).ToArray()
        };
    }

    /// <inheritdoc cref="ICatalogClient.GetAssetTagsAsync"/>
    public Task<IReadOnlyCollection<string>> GetAssetTagsAsync(long assetId, CancellationToken cancellationToken)
    {
        return _AssetTagsClient.GetAsync(assetId, cancellationToken);
    }

    /// <inheritdoc cref="ICatalogClient.SearchAsync"/>
    public Task<PagedResult<CatalogSearchResult>> SearchAsync(CatalogSearchRequest searchRequest, string cursor, CancellationToken cancellationToken)
    {
        if (searchRequest == null)
        {
            throw new ArgumentNullException(nameof(searchRequest));
        }

        var queryParameters = new Dictionary<string, string>
        {
            ["limit"] = Paging.Limit,
            ["cursor"] = cursor ?? string.Empty,
            ["includeNotForSale"] = searchRequest.IncludeOffSaleItems.ToString().ToLower()
        };

        if (!string.IsNullOrWhiteSpace(searchRequest.Category))
        {
            queryParameters["category"] = searchRequest.Category;
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.Subcategory))
        {
            queryParameters["subcategory"] = searchRequest.Subcategory;
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.SortType))
        {
            queryParameters["sortType"] = searchRequest.SortType;
        }

        if (!string.IsNullOrWhiteSpace(searchRequest.CreatorName))
        {
            queryParameters["creatorName"] = searchRequest.CreatorName;
        }

        return _HttpClient.SendApiRequestAsync<PagedResult<CatalogSearchResult>>(HttpMethod.Get, RobloxDomain.CatalogApi, $"v1/search/items", queryParameters, cancellationToken);
    }

    private async Task<IReadOnlyDictionary<long, CatalogAssetDetails>> MultiGetAssetsAsync(IReadOnlyCollection<long> assetIds, CancellationToken cancellationToken)
    {
        var requestBody = new CatalogItemDetailsRequest
        {
            Data = assetIds.Select(id => new CatalogItemDetailsRequestItem
            {
                Id = id,
                Type = "Asset"
            }).ToArray()
        };

        var pagedResult = await _HttpClient.SendApiRequestAsync<CatalogItemDetailsRequest, PagedResult<CatalogItemDetailsResult>>(HttpMethod.Post, RobloxDomain.CatalogApi, $"v1/catalog/items/details", queryParameters: null, requestBody, cancellationToken);
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
                    CountRemaining = asset.CountRemaining,
                    OffSaleDateTime = asset.OffSaleDateTime
                } : null;

                if (productDetails?.Price == 0 && asset.PriceStatus != "Free")
                {
                    productDetails.Price = null;
                }

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
        var pagedResult = await _HttpClient.SendApiRequestAsync<CatalogBundleDetailsResult[]>(HttpMethod.Get, RobloxDomain.CatalogApi, $"v1/bundles/details", queryParameters: new Dictionary<string, string>
        {
            ["bundleIds"] = string.Join(',', bundleIds)
        }, cancellationToken);
        var bundlesById = pagedResult.ToDictionary(b => b.Id, b => b);
        var result = new Dictionary<long, CatalogBundleDetails>();

        foreach (var id in bundleIds)
        {
            if (bundlesById.TryGetValue(id, out var bundle))
            {
                result[id] = TranslateBundle(bundle);
            }
            else
            {
                result[id] = null;
            }
        }

        return result;
    }

    private async Task<IReadOnlyDictionary<long, IReadOnlyCollection<string>>> MultiGetAssetTagsAsync(IReadOnlyCollection<long> assetIds, CancellationToken cancellationToken)
    {
        var pagedResult = await _HttpClient.SendApiRequestAsync<PagedResult<ItemTagsResult>>(HttpMethod.Get, RobloxDomain.ItemConfigurationApi, $"v1/item-tags", queryParameters: new Dictionary<string, string>
        {
            ["itemIds"] = string.Join(',', assetIds.Select(id => $"AssetId:{id}"))
        }, cancellationToken);
        var tagsByAssetId = pagedResult.Data.Where(r => r.AssetId.HasValue).ToDictionary(r => r.AssetId.Value, r => r.Tags.Select(t => t.Tag.Name).ToArray());
        var result = new Dictionary<long, IReadOnlyCollection<string>>();

        foreach (var assetId in assetIds)
        {
            if (tagsByAssetId.TryGetValue(assetId, out var tags))
            {
                result[assetId] = tags;
            }
            else
            {
                result[assetId] = Array.Empty<string>();
            }
        }

        return result;
    }

    private CatalogBundleDetails TranslateBundle(CatalogBundleDetailsResult bundle)
    {
        CatalogItemDetailsProduct productDetails = null;

        if (bundle.Product != null)
        {
            productDetails = new CatalogItemDetailsProduct
            {
                Id = bundle.Product.Id
            };

            if (bundle.Product.Free)
            {
                productDetails.Price = 0;
            }
            else if (bundle.Product.ForSale)
            {
                productDetails.Price = bundle.Product.Price;
            }
        }

        return new CatalogBundleDetails
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
}
