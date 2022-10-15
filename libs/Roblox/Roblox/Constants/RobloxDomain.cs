using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Roblox.Api;

/// <summary>
/// Domain names Roblox uses.
/// </summary>
public static class RobloxDomain
{
    /// <summary>
    /// The economy API.
    /// </summary>
    public const string EconomyApi = "https://economy.roblox.com";

    /// <summary>
    /// The friends API.
    /// </summary>
    public const string FriendsApi = "https://friends.roblox.com";

    /// <summary>
    /// The users API.
    /// </summary>
    public const string UsersApi = "https://users.roblox.com";

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
}
