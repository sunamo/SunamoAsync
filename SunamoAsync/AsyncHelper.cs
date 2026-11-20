namespace SunamoAsync;

/// <summary>
/// Represents the async helper type.
/// </summary>
public partial class AsyncHelper
{
    /// <summary>
    /// The instance.
    /// </summary>
    public static AsyncHelper Instance = new();
    private AsyncHelper()
    {
    }

    /// <summary>
    /// Invoke func task or action.
    /// </summary>
    /// <param name="actionOrFunc">The action or func.</param>
    /// <returns>The invoke func task or action result.</returns>
    public static async Task InvokeFuncTaskOrAction(object actionOrFunc)
    {
        var objectType = actionOrFunc.GetType();
        if (objectType == typeof(Action))
        {
            (actionOrFunc as Action)?.Invoke();
        }
        else if (objectType == typeof(Func<Task>))
        {
            var taskVoid = actionOrFunc as Func<Task>;
            if (taskVoid != null)
            {
                await
                taskVoid();
            }
        }
    }

    /// <summary>
    /// Get result t.
    /// </summary>
    /// <typeparam name="T">The type T.</typeparam>
    /// <param name="task">The task.</param>
    /// <returns>The get result t result.</returns>
    public T GetResult<T>(Task<T> task)
    {
        task.LogExceptions();
        var configuredTask = task.Conf();
        var taskAwaiter = configuredTask.GetAwaiter();
        return taskAwaiter.GetResult();
    }

    /// <summary>
    /// Get result.
    /// </summary>
    /// <returns>The get result result.</returns>
    public void GetResult(Task task)
    {
        task.LogExceptions();
        task.Conf();
    }

    /// <summary>
    /// Initializes a new instance of the run async class.
    /// </summary>
    public async Task RunAsync(Task task)
    {
        await task;
    }

    /// <summary>
    /// Run sync t t 1.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="argument1">The argument 1.</param>
    /// <returns>The run sync t t 1 result.</returns>
    public T RunSync<T, T1>(Func<T1, T> task, T1 argument1)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        T result = default!;
        synch.Post(_ =>
        {
            try
            {
                result = task(argument1);
            }
            catch (Exception exception)
            {
                synch.InnerException = exception;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        synch.Dispose();
        return result;
    }

    /// <summary>
    /// Run sync t t 1 t 2.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="argument1">The argument 1.</param>
    /// <param name="argument2">The argument 2.</param>
    /// <returns>The run sync t t 1 t 2 result.</returns>
    public T? RunSync<T, T1, T2>(Func<T1, T2, T> task, T1 argument1, T2 argument2)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        T? result = default;
        synch.Post(_ =>
        {
            try
            {
                result = task(argument1, argument2);
            }
            catch (Exception exception)
            {
                synch.InnerException = exception;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        synch.Dispose();
        return result;
    }

    /// <summary>
    /// Run sync t t 1 t 2 t 3.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <param name="argument1">The argument 1.</param>
    /// <param name="argument2">The argument 2.</param>
    /// <param name="argument3">The argument 3.</param>
    /// <returns>The run sync t t 1 t 2 t 3 result.</returns>
    public T RunSync<T, T1, T2, T3>(Func<T1, T2, T3, T> task, T1 argument1, T2 argument2, T3 argument3)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        T result = default!;
        synch.Post(_ =>
        {
            try
            {
                result = task(argument1, argument2, argument3);
            }
            catch (Exception exception)
            {
                synch.InnerException = exception;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        synch.Dispose();
        return result;
    }

    /// <summary>
    /// Run sync without return value.
    /// </summary>
    /// <param name="task">The task.</param>
    /// <returns>The run sync without return value result.</returns>
    public void RunSyncWithoutReturnValue(Func<Task> task)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(_ =>
        {
            try
            {
                Instance.GetResult(task());
            }
            catch (Exception exception)
            {
                synch.InnerException = exception;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        synch.Dispose();
    }

    /// <summary>
    /// Initializes a new instance of the run sync without return value t 1 class.
    /// </summary>
    public void RunSyncWithoutReturnValue<T1>(Func<T1, Task> task, T1 argument1)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(_ =>
        {
            try
            {
                Instance.GetResult(task(argument1));
            }
            catch (Exception exception)
            {
                synch.InnerException = exception;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
    }

    /// <summary>
    /// Initializes a new instance of the run sync without return value t 1 t 2 class.
    /// </summary>
    /// <param name="oldContext">The old context.</param>
    public void RunSyncWithoutReturnValue<T1, T2>(Func<T1, T2, Task> task, T1 argument1, T2 argument2)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(_ =>
        {
            try
            {
                Instance.GetResult(task(argument1, argument2));
            }
            catch (Exception exception)
            {
                synch.InnerException = exception;
                throw;
            }
            finally
            {
                synch.EndMessageLoop();
            }
        }, null);
        synch.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        synch.Dispose();
    }
}
