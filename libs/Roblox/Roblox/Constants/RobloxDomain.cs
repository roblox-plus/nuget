using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace Roblox.Api;

/// <summary>
/// Domain names Roblox uses.
/// </summary>
public static class RobloxDomain
{
    private static readonly Regex _UrlNameRegex = new(@"\W+");

    /// <summary>
    /// The Roblox domain.
    /// </summary>
    public const string Value = ".roblox.com";

    /// <summary>
    /// Strange apis domain, not to be confused with the also existing "api" domain. Sure.
    /// </summary>
    public const string Apis = "https://apis.roblox.com";

    /// <summary>
    /// The avatar API.
    /// </summary>
    public const string AvatarApi = "https://avatar.roblox.com";

    /// <summary>
    /// The catalog API.
    /// </summary>
    public const string CatalogApi = "https://catalog.roblox.com";

    /// <summary>
    /// The economy API.
    /// </summary>
    public const string EconomyApi = "https://economy.roblox.com";

    /// <summary>
    /// The friends API.
    /// </summary>
    public const string FriendsApi = "https://friends.roblox.com";

    /// <summary>
    /// The groups API.
    /// </summary>
    public const string GroupsApi = "https://groups.roblox.com";

    /// <summary>
    /// The inventory API.
    /// </summary>
    public const string InventoryApi = "https://inventory.roblox.com";

    /// <summary>
    /// The item configuration API.
    /// </summary>
    public const string ItemConfigurationApi = "https://itemconfiguration.roblox.com";

    /// <summary>
    /// The thumbnails API.
    /// </summary>
    public const string ThumbnailsApi = "https://thumbnails.roblox.com";

    /// <summary>
    /// The users API.
    /// </summary>
    public const string UsersApi = "https://users.roblox.com";

    /// <summary>
    /// The main website.
    /// </summary>
    public const string Website = "https://www.roblox.com";

    /// <summary>
    /// Builds a named Roblox website URL.
    /// </summary>
    /// <param name="robloxEntity">The <see cref="RobloxEntity"/>.</param>
    /// <param name="id">The ID of the item.</param>
    /// <param name="name">The name of the item.</param>
    /// <returns>The <see cref="Uri"/>.</returns>
    /// <exception cref="ArgumentException">
    /// - <paramref name="robloxEntity"/> is invalid.
    /// </exception>
    public static Uri BuildWebsiteItemUrl(string robloxEntity, long id, string name)
    {
        var seoName = robloxEntity == RobloxEntity.User ? "profile" : GetUrlName(name);
        return new Uri($"{Website}/{TranslateRobloxEntity(robloxEntity)}/{id}/{seoName}");
    }

    /// <summary>
    /// Builds a URI, given a domain, path, and query string parameters.
    /// </summary>
    /// <param name="domain">The domain to use for the <seealso cref="Uri"/>.</param>
    /// <param name="path">The request path.</param>
    /// <param name="queryParameters">The query parameters.</param>
    /// <returns>The built <seealso cref="Uri"/>.</returns>
    public static Uri Build(string domain, string path, IReadOnlyDictionary<string, string> queryParameters = null)
    {
        if (string.IsNullOrWhiteSpace(domain))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(domain));
        }

        var uriBuilder = new UriBuilder($"{domain.TrimEnd('/')}/{path?.TrimStart('/')}");

        if (queryParameters?.Any() == true)
        {
            var query = HttpUtility.ParseQueryString(uriBuilder.Query);

            foreach (var parameter in queryParameters)
            {
                query[parameter.Key] = parameter.Value;
            }

            uriBuilder.Query = query.ToString();
        }

        return uriBuilder.Uri;
    }

    private static string TranslateRobloxEntity(string robloxEntity)
    {
        switch (robloxEntity)
        {
            case RobloxEntity.Group:
                return "groups";
            case RobloxEntity.Asset:
                return "catalog";
            case RobloxEntity.User:
                return "users";
            case RobloxEntity.Bundle:
                return "bundles";
            case RobloxEntity.GamePass:
                return "game-pass";
            case RobloxEntity.Badge:
                return "badges";
            default:
                throw new ArgumentException($"Unknown Roblox entity type: {robloxEntity}", nameof(robloxEntity));
        }
    }

    private static string GetUrlName(string name)
    {
        var strippedName = name.Replace("'", string.Empty).Trim();
        var replacedName = _UrlNameRegex.Replace(strippedName, "-");
        var finalName = replacedName.Trim('-');

        if (string.IsNullOrWhiteSpace(finalName))
        {
            return "unnamed";
        }

        return finalName;
    }
}
