using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Api;

/// <inheritdoc cref="IBatchClient{TId,TResult}"/>
internal class BatchingClient<TId, TResult> : IBatchClient<TId, TResult>, IDisposable
    where TId : IComparable
    where TResult : class
{
    private static readonly TimeSpan _MinimumSendInterval = TimeSpan.FromMilliseconds(500);
    private readonly ConcurrentBag<(TId Id, TaskCompletionSource<TResult> Result)> _Requests = new();
    private readonly Func<IReadOnlyCollection<TId>, CancellationToken, Task<IReadOnlyDictionary<TId, TResult>>> _FetchAsync;
    private readonly int _BatchSize;
    private readonly TimeSpan _Throttle;
    private readonly TimeSpan _SendInterval;
    private readonly Timer _SendTimer;
    private readonly SemaphoreSlim _ProcessLock = new(1, 1);
    private DateTime _LastSend = DateTime.MinValue;

    /// <inheritdoc cref="IBatchClient{TId,TResult}.Size"/>
    public int Size => _Requests.Count;

    /// <summary>
    /// Initializes a new <seealso cref="BatchingClient{TId,TResult}"/>.
    /// </summary>
    /// <param name="fetchAsync">A function for fetching a batch of items, given the IDs for those items.</param>
    /// <param name="batchSize">The number of items to put into a single batch.</param>
    /// <param name="throttle">The minimum time to wait between sending requests.</param>
    /// <param name="sendInterval">The maximum time to wait between sending requests.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// - <paramref name="batchSize"/> cannot be below 1.
    /// </exception>
    /// <exception cref="ArgumentNullException">
    /// - <paramref name="fetchAsync"/>
    /// </exception>
    public BatchingClient(Func<IReadOnlyCollection<TId>, CancellationToken, Task<IReadOnlyDictionary<TId, TResult>>> fetchAsync, int batchSize, TimeSpan throttle, TimeSpan sendInterval)
    {
        if (batchSize <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(batchSize));
        }

        _BatchSize = batchSize;
        _Throttle = throttle;
        _SendInterval = sendInterval;
        _FetchAsync = fetchAsync ?? throw new ArgumentNullException(nameof(fetchAsync));

        _SendTimer = new Timer(_ => TryProcessAsync().GetAwaiter().GetResult(), state: null, _MinimumSendInterval, _MinimumSendInterval);
    }

    /// <inheritdoc cref="IBatchClient{TId,TResult}.GetAsync"/>
    public Task<TResult> GetAsync(TId id, CancellationToken cancellationToken)
    {
        return Task.Run(async () =>
        {
            var existingRequest = _Requests.FirstOrDefault(r => r.Id.Equals(id));
            if (!existingRequest.Equals(default))
            {
                // We're already waiting for this request.
                return await existingRequest.Result.Task;
            }

            var task = new TaskCompletionSource<TResult>();
            _Requests.Add((id, task));

            await TryProcessAsync();

            return await task.Task;
        }, cancellationToken);
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _SendTimer?.Dispose();
        _ProcessLock?.Dispose();
    }

    private async Task TryProcessAsync()
    {
        if (!ShouldProcess())
        {
            return;
        }

        _LastSend = DateTime.UtcNow;
        await _ProcessLock.WaitAsync();

        try
        {
            var requestsById = new Dictionary<TId, TaskCompletionSource<TResult>>();
            while (requestsById.Count < _BatchSize && _Requests.TryTake(out var request))
            {
                if (requestsById.ContainsKey(request.Id))
                {
                    // We somehow already have the ID in the dictionary for what needs to be requested.
                    // This means we somehow ended up with a duplicate ID in the bag.
                    // We're going to put it back so it doesn't get lost, since we key on this ID.
                    _Requests.Add(request);

                    // Because a ConcurrentBag is unordered, there's no guarantee that putting it back
                    // will result in the request not just coming back out with the next TryTake.
                    // We must break to avoid this request getting lost, or ending up in an endless loop.
                    break;
                }

                requestsById[request.Id] = request.Result;
            }

            if (!requestsById.Any())
            {
                return;
            }

            try
            {
                var results = await _FetchAsync(requestsById.Keys, CancellationToken.None);
                foreach (var (id, task) in requestsById)
                {
                    if (results.TryGetValue(id, out var result))
                    {
                        task.SetResult(result);
                    }
                    else
                    {
                        task.SetException(new KeyNotFoundException($"{nameof(BatchingClient<TId, TResult>)} did not return result for item"));
                    }
                }
            }
            catch (Exception e)
            {
                foreach (var (_, task) in requestsById)
                {
                    task.SetException(e);
                }
            }
        }
        finally
        {
            _ProcessLock.Release();
        }
    }

    private bool ShouldProcess()
    {
        if (Size < 1)
        {
            // Nothing to process.
            return false;
        }

        if (_ProcessLock.CurrentCount == 0)
        {
            // Lock is taken.
            return false;
        }

        if (_LastSend + _Throttle > DateTime.UtcNow)
        {
            // We've hit the max amount of requests we can send, we're not allowed to proceed.
            return false;
        }

        if (Size >= _BatchSize)
        {
            // We have enough requests for a full batch, we should process them.
            return true;
        }

        if (_LastSend < DateTime.UtcNow - _SendInterval)
        {
            // We have hit the maximum time we're allowed to wait to send the request.
            return true;
        }

        // We haven't satisfied any conditions required to process the request.
        return false;
    }
}
