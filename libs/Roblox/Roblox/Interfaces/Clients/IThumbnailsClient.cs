using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Thumbnails;

/// <summary>
/// A client for fetching thumbnails.
/// </summary>
public interface IThumbnailsClient
{
    /// <summary>
    /// Fetches the 2D thumbnail for an asset.
    /// </summary>
    /// <param name="assetId">The asset ID.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The thumbnail URL, or <c>null</c> if the thumbnail has not been generated yet.</returns>
    Task<ThumbnailResult> GetAssetThumbnailAsync(long assetId, CancellationToken cancellationToken);

    /// <summary>
    /// Fetches the 2D thumbnail for a bundle.
    /// </summary>
    /// <param name="bundleId">The bundle ID.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The thumbnail URL, or <c>null</c> if the thumbnail has not been generated yet.</returns>
    Task<ThumbnailResult> GetBundleThumbnailAsync(long bundleId, CancellationToken cancellationToken);
}
