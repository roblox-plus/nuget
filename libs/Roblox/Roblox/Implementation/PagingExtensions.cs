using System.Collections.Generic;
using System.ComponentModel;

namespace Roblox.Api;

/// <summary>
/// Extension methods for API paging parameters.
/// </summary>
public static class PagingExtensions
{
    /// <summary>
    /// Translates a <seealso cref="ListSortDirection"/> to a value to send to Roblox APIs.
    /// </summary>
    /// <param name="sortOrder">The <seealso cref="ListSortDirection"/>.</param>
    /// <returns>"Desc" if <see cref="ListSortDirection.Descending"/>, otherwise "Asc".</returns>
    /// <exception cref="InvalidEnumArgumentException">
    /// - <paramref name="sortOrder"/>
    /// </exception>
    internal static string ToApiSortOrder(this ListSortDirection sortOrder)
    {
        switch (sortOrder)
        {
            case ListSortDirection.Ascending:
                return "Asc";
            case ListSortDirection.Descending:
                return "Desc";
            default:
                throw new InvalidEnumArgumentException(nameof(sortOrder), (int)sortOrder, typeof(ListSortDirection));
        }
    }

    /// <summary>
    /// Creates the query parameters dictionary containing paging parameters.
    /// </summary>
    /// <param name="cursor">The paging cursor.</param>
    /// <param name="sortOrder">The sort order for the request.</param>
    /// <returns>The paging parameters.</returns>
    internal static Dictionary<string, string> ToPagingParameters(this string cursor, ListSortDirection sortOrder)
    {
        return new Dictionary<string, string>
        {
            ["cursor"] = cursor,
            ["sortOrder"] = sortOrder.ToApiSortOrder(),
            ["limit"] = Paging.Limit
        };
    }
}
