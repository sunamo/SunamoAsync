// variables names: ok
namespace SunamoAsync;

// variables names: ok
// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
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
    ///     To all regions insert comments whats not and what working
    ///     Not working with Directory.GetFilesMoreMascAsync - with use https://stackoverflow.com/a/34518914 OK
    ///     Task.Run&lt;&gt;(async () =&gt; await FunctionAsync()).Result;
    /// </summary>
    /// <typeparam name = "T">The type of the task result.</typeparam>
    /// <param name = "task">The task to get result from.</param>
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
    ///     Execute's an T> method which has a T return type synchronously
    /// </summary>
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
    ///     Execute's an T> method which has a T return type synchronously
    /// </summary>
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
    ///     Execute's an T> method which has a T return type synchronously
    /// </summary>
    /// <typeparam name = "T">Return Type</typeparam>
    /// <typeparam name = "T1">First parameter type</typeparam>
    /// <typeparam name = "T2">Second parameter type</typeparam>
    /// <typeparam name = "T3">Third parameter type</typeparam>
    /// <param name = "task">T> method to execute</param>
    /// <param name = "argument1">First argument</param>
    /// <param name = "argument2">Second argument</param>
    /// <param name = "argument3">Third argument</param>
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
    ///     Execute's an T> method which has a void return value synchronously
    /// </summary>
    /// <param name = "task">T> method to execute</param>
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
    ///     Execute's an async method with one parameter synchronously without return value
    /// </summary>
    /// <typeparam name = "T1">First parameter type</typeparam>
    /// <param name = "task">Async method to execute</param>
    /// <param name = "argument1">First argument</param>
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
    ///     Execute's an async method with two parameters synchronously without return value
    /// </summary>
    /// <typeparam name = "T1">First parameter type</typeparam>
    /// <typeparam name = "T2">Second parameter type</typeparam>
    /// <param name = "task">Async method to execute</param>
    /// <param name = "argument1">First argument</param>
    /// <param name = "argument2">Second argument</param>
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