namespace SunamoAsync._sunamo.SunamoExtensions;

internal static class TaskExtensions
{
    #region For easy copy from TaskExtensionsSunamo.cs
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
                var aggException = completedTask.Exception.Flatten();
                throw new Exception(Exceptions.TextOfExceptions(aggException));
            },
            TaskContinuationOptions.OnlyOnFaulted);
    }
    #endregion
}