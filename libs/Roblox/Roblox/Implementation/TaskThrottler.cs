using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Api;

/// <inheritdoc cref="ITaskThrottler{TResult}"/>
internal class TaskThrottler<TResult> : ITaskThrottler<TResult>, IDisposable
{
    private readonly ConcurrentQueue<(TaskCompletionSource<TResult> Task, Func<Task<TResult>> Action)> _TaskQueue = new();
    private readonly Timer _TaskTimer;

    /// <summary>
    /// Initializes a new <see cref="TaskThrottler{TResult}"/>.
    /// </summary>
    /// <param name="throttle">How long to throttle the tasks for.</param>
    public TaskThrottler(TimeSpan throttle)
    {
        _TaskTimer = new Timer(ProcessTaskAsync, state: null, throttle, throttle);
    }

    /// <inheritdoc cref="ITaskThrottler{TResult}.RunAsync"/>
    public Task<TResult> RunAsync(Func<Task<TResult>> action, CancellationToken cancellationToken)
    {
        var completionSource = new TaskCompletionSource<TResult>(cancellationToken);
        _TaskQueue.Enqueue((completionSource, action));
        return completionSource.Task;
    }

    /// <inheritdoc cref="IDisposable.Dispose"/>
    public void Dispose()
    {
        _TaskTimer?.Dispose();
    }

    private async void ProcessTaskAsync(object _)
    {
        if (!_TaskQueue.TryDequeue(out var task))
        {
            return;
        }

        try
        {
            var result = await task.Action();
            task.Task.SetResult(result);
        }
        catch (Exception ex)
        {
            task.Task.SetException(ex);
        }
    }
}
