using System;
using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Api;

/// <summary>
/// A client intended for batching multiple read calls together into a single, batched request.
/// </summary>
/// <typeparam name="TId">The identifier for fetching the item.</typeparam>
/// <typeparam name="TResult">The actual item that was fetched.</typeparam>
internal interface IBatchClient<TId, TResult>
    where TId : IComparable
    where TResult : class
{
    /// <summary>
    /// The current size of the queue of how many requests are waiting to be sent.
    /// </summary>
    int Size { get; }

    /// <summary>
    /// Adds an item to the batching queue, and waits for the batched request to be processed.
    /// </summary>
    /// <param name="id">The ID of the item being requested.</param>
    /// <param name="cancellationToken">A <seealso cref="CancellationToken"/>.</param>
    /// <returns>The requested item.</returns>
    public Task<TResult> GetAsync(TId id, CancellationToken cancellationToken);
}

