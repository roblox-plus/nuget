using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Roblox.Api;

namespace Roblox.Catalog;

/// <summary>
/// A client for accessing information from the Roblox avatar catalog.
/// </summary>
public interface ICatalogClient
{
    /// <summary>
    /// Gets asset details by its ID.
    /// </summary>
    /// <param name="assetId">The asset ID.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="CatalogAssetDetails"/>, or <c>null</c> if the asset does not exist.</returns>
    Task<CatalogAssetDetails> GetAssetAsync(long assetId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets bundle details by its ID.
    /// </summary>
    /// <param name="bundleId">The bundle ID.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="CatalogAssetDetails"/>, or <c>null</c> if the asset does not exist.</returns>
    Task<CatalogBundleDetails> GetBundleAsync(long bundleId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets all the bundles an asset is apart of.
    /// </summary>
    /// <param name="assetId">The asset ID.</param>
    /// <param name="cursor">The cursor to use to page through the results.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="PagedResult{TData}"/> of <see cref="CatalogBundleDetails"/>.</returns>
    Task<PagedResult<CatalogBundleDetails>> GetBundlesByAssetIdAsync(long assetId, string cursor, CancellationToken cancellationToken);

    /// <summary>
    /// Gets the tags associated with an asset.
    /// </summary>
    /// <param name="assetId">The asset ID.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The tags.</returns>
    Task<IReadOnlyCollection<string>> GetAssetTagsAsync(long assetId, CancellationToken cancellationToken);
}
