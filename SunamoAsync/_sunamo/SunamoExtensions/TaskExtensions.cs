// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoAsync._sunamo.SunamoExtensions;

internal static class TaskExtensions
{
    #region For easy copy from TaskExtensionsSunamo.cs
    internal static ConfiguredTaskAwaitable Conf(this Task t)
    {
        return t.ConfigureAwait(true);
    }

    internal static ConfiguredTaskAwaitable<T> Conf<T>(this Task<T> t)
    {
        return t.ConfigureAwait(true);
    }

    internal static void LogExceptions(this Task task)
    {
        task.ContinueWith(t =>
            {
                var aggException = t.Exception.Flatten();
                throw new Exception(Exceptions.TextOfExceptions(aggException));
            },
            TaskContinuationOptions.OnlyOnFaulted);
    }
    #endregion
}
