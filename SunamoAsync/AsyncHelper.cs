namespace SunamoAsync;

// variables names: ok
/// <summary>
/// Helper class for running async methods synchronously.
/// </summary>
public partial class AsyncHelper
{
    /// <summary>
    /// Singleton instance of AsyncHelper.
    /// </summary>
    public static AsyncHelper Instance = new();
    private AsyncHelper()
    {
    }

    /// <summary>
    /// Invokes either an Action or a Func&lt;Task&gt; asynchronously.
    /// </summary>
    /// <param name="actionOrFunc">The action or function to invoke.</param>
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
#if ASYNC
                await
#endif
                taskVoid();
            }
        }
    }

    /// <summary>
    /// Gets the result of a typed task synchronously.
    /// </summary>
    /// <typeparam name="T">The type of the task result.</typeparam>
    /// <param name="task">The task to get result from.</param>
    public T GetResult<T>(Task<T> task)
    {
        task.LogExceptions();
        var configuredTask = task.Conf();
        var taskAwaiter = configuredTask.GetAwaiter();
        return taskAwaiter.GetResult();
    }

    /// <summary>
    /// Gets the result of a task synchronously.
    /// </summary>
    /// <param name="task">The task to get result from.</param>
    public void GetResult(Task task)
    {
        task.LogExceptions();
        task.Conf();
    }

    /// <summary>
    /// Runs a task asynchronously.
    /// </summary>
    /// <param name="task">The task to run.</param>
    public async Task RunAsync(Task task)
    {
        await task;
    }

    /// <summary>
    /// Executes a function with one parameter synchronously and returns its result.
    /// </summary>
    /// <typeparam name="T">Return type.</typeparam>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <param name="task">Function to execute.</param>
    /// <param name="argument1">First argument.</param>
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
    /// Executes a function with two parameters synchronously and returns its result.
    /// </summary>
    /// <typeparam name="T">Return type.</typeparam>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <param name="task">Function to execute.</param>
    /// <param name="argument1">First argument.</param>
    /// <param name="argument2">Second argument.</param>
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
    /// Executes a function with three parameters synchronously and returns its result.
    /// </summary>
    /// <typeparam name="T">Return type.</typeparam>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <typeparam name="T3">Third parameter type.</typeparam>
    /// <param name="task">Function to execute.</param>
    /// <param name="argument1">First argument.</param>
    /// <param name="argument2">Second argument.</param>
    /// <param name="argument3">Third argument.</param>
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
    /// Executes an async method synchronously without return value.
    /// </summary>
    /// <param name="task">Async method to execute.</param>
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
    /// Executes an async method with one parameter synchronously without return value.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <param name="task">Async method to execute.</param>
    /// <param name="argument1">First argument.</param>
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
    /// Executes an async method with two parameters synchronously without return value.
    /// </summary>
    /// <typeparam name="T1">First parameter type.</typeparam>
    /// <typeparam name="T2">Second parameter type.</typeparam>
    /// <param name="task">Async method to execute.</param>
    /// <param name="argument1">First argument.</param>
    /// <param name="argument2">Second argument.</param>
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