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
    /// <returns>The <see cref="ThumbnailResult"/>.</returns>
    Task<ThumbnailResult> GetAssetThumbnailAsync(long assetId, CancellationToken cancellationToken);

    /// <summary>
    /// Fetches the 2D thumbnail for a bundle.
    /// </summary>
    /// <param name="bundleId">The bundle ID.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ThumbnailResult"/>.</returns>
    Task<ThumbnailResult> GetBundleThumbnailAsync(long bundleId, CancellationToken cancellationToken);

    /// <summary>
    /// Renders a full avatar thumbnail for a user, wearing additional assets.
    /// </summary>
    /// <param name="userId">The ID of the user, to use as the base for the avatar render.</param>
    /// <param name="assetIds">The assets to render on the avatar.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="ThumbnailResult"/>.</returns>
    Task<ThumbnailResult> RenderAvatarAsync(long userId, long[] assetIds, CancellationToken cancellationToken);
}
