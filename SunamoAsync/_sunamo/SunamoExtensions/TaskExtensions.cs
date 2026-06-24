namespace SunamoAsync._sunamo.SunamoExtensions;

internal static class TaskExtensions
{
    internal static ConfiguredTaskAwaitable Conf(this Task task)
    {
        return task.ConfigureAwait(true);
    }

    internal static ConfiguredTaskAwaitable<T> Conf<T>(this Task<T> task)
    {
        return task.ConfigureAwait(true);
    }

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
}
