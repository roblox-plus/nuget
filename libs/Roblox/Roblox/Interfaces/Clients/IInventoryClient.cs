using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Roblox.Api;

namespace Roblox.Inventory;

/// <summary>
/// A client for fetching user inventory information.
/// </summary>
public interface IInventoryClient
{
    /// <summary>
    /// Gets ownership records for an asset.
    /// </summary>
    /// <param name="assetId">The ID of the asset to get ownership records for.</param>
    /// <param name="cursor">The cursor, used to page through ownership records.</param>
    /// <param name="sortOrder">The sort order for the request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="PagedResult{TData}"/> of <see cref="AssetOwnershipResult"/>.</returns>
    Task<PagedResult<AssetOwnershipResult>> GetAssetOwnersAsync(long assetId, string cursor, ListSortDirection sortOrder, CancellationToken cancellationToken);

    /// <summary>
    /// Gets bundle ownership records for a user.
    /// </summary>
    /// <param name="userId">The ID of the user.</param>
    /// <param name="cursor">The cursor, used to page through ownership records.</param>
    /// <param name="sortOrder">The sort order for the request.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>A <see cref="PagedResult{TData}"/> of <see cref="BundleOwnershipResult"/>.</returns>
    Task<PagedResult<BundleOwnershipResult>> GetOwnedBundlesByUserIdAsync(long userId, string cursor, ListSortDirection sortOrder, CancellationToken cancellationToken);
}
