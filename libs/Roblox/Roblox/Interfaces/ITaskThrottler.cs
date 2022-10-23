using System;
using System.Threading;
using System.Threading.Tasks;

namespace Roblox.Api;

/// <summary>
/// Throttles tasks from executing.
/// </summary>
/// <remarks>
/// The intention of this class is not to execute the tasks that run through it more than
/// once per the specified time interval.
/// </remarks>
/// <typeparam name="TResult">The task result.</typeparam>
public interface ITaskThrottler<TResult>
{
    /// <summary>
    /// Runs a task, after the throttling period.
    /// </summary>
    /// <param name="action">The action to run in the task.</param>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/>.</param>
    /// <returns>The <see cref="Task{TResult}"/>, which will be run after the throttling period.</returns>
    Task<TResult> RunAsync(Func<Task<TResult>> action, CancellationToken cancellationToken);
}
