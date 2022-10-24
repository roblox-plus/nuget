using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace Roblox.Api;

/// <summary>
/// Extension methods for <see cref="AssetType"/>.
/// </summary>
public static class AssetTypeExtensions
{
    /// <summary>
    /// Gets the display name for an asset type, to present to a human.
    /// </summary>
    /// <param name="assetType">The <see cref="AssetType"/>.</param>
    /// <returns>The display name.</returns>
    public static string GetDisplayName(this AssetType assetType)
    {
        var defaultName = assetType.ToString();
        var displayAttribute = typeof(AssetType).GetField(defaultName)?.GetCustomAttribute<DisplayAttribute>();

        if (!string.IsNullOrWhiteSpace(displayAttribute?.Name))
        {
            return displayAttribute.Name;
        }

        return defaultName;
    }
}
