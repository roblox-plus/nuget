using System.Threading;
using System.Threading.Tasks;

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
    /// <returns>The <seealso cref="CatalogAssetDetails"/>, or <c>null</c> if the asset does not exist.</returns>
    Task<CatalogAssetDetails> GetAssetAsync(long assetId, CancellationToken cancellationToken);

    /// <summary>
    /// Gets bundle details by its ID.
    /// </summary>
    /// <param name="bundleId">The bundle ID.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The <seealso cref="CatalogAssetDetails"/>, or <c>null</c> if the asset does not exist.</returns>
    Task<CatalogBundleDetails> GetBundleAsync(long bundleId, CancellationToken cancellationToken);
}
