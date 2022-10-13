using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Roblox.Api;

/// <summary>
/// A paged result model, from Roblox APIs.
/// </summary>
/// <typeparam name="TData">The type of data in the paged result.</typeparam>
[DataContract]
public class PagedResult<TData>
{
    /// <summary>
    /// A cursor to select the next page of results with.
    /// </summary>
    [DataMember(Name = "nextPageCursor")]
    public string NextPageCursor { get; set; }

    /// <summary>
    /// A cursor to select the previous page of results with.
    /// </summary>
    [DataMember(Name = "previousPageCursor")]
    public string PreviousNextCursor { get; set; }

    /// <summary>
    /// The paged items.
    /// </summary>
    [DataMember(Name = "data")]
    public IReadOnlyCollection<TData> Data { get; set; }
}
