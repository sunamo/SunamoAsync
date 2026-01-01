namespace SunamoAsync._sunamo.SunamoExtensions;

internal static class TaskExtensions
{
    #region For easy copy from TaskExtensionsSunamo.cs
    /// <summary>
    /// Configures the task to continue on the captured context.
    /// </summary>
    /// <param name="task">The task to configure.</param>
    /// <returns>Configured task awaitable.</returns>
    internal static ConfiguredTaskAwaitable Conf(this Task task)
    {
        return task.ConfigureAwait(true);
    }

    /// <summary>
    /// Configures the task to continue on the captured context.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to configure.</param>
    /// <returns>Configured task awaitable.</returns>
    internal static ConfiguredTaskAwaitable<T> Conf<T>(this Task<T> task)
    {
        return task.ConfigureAwait(true);
    }

    /// <summary>
    /// Logs exceptions that occur in the task.
    /// </summary>
    /// <param name="task">The task to monitor for exceptions.</param>
    internal static void LogExceptions(this Task task)
    {
        task.ContinueWith(completedTask =>
            {
                var aggregatedException = completedTask.Exception?.Flatten();
                if (aggregatedException != null)
                {
                    throw new Exception(Exceptions.TextOfExceptions(aggregatedException));
                }
            },
            TaskContinuationOptions.OnlyOnFaulted);
    }
    #endregion
}