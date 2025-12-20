// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoAsync;
public partial class AsyncHelper
{
    public static AsyncHelper ci = new();
    private AsyncHelper()
    {
    }

    public static async Task InvokeFuncTaskOrAction(object actionOrFunc)
    {
        var objectType = actionOrFunc.GetType();
        if (objectType == typeof(Action))
        {
            (actionOrFunc as Action).Invoke();
        }
        else if (objectType == typeof(Func<Task>))
        {
            var taskVoid = actionOrFunc as Func<Task>;
#if ASYNC
            await
#endif
            taskVoid();
            ;
        }
    }

    /// <summary>
    ///     To all regions insert comments whats not and what working
    ///     Not working with Directory.GetFilesMoreMascAsync - with use https://stackoverflow.com/a/34518914 OK
    ///     Task.Run<>(async () => await FunctionAsync()).Result;
    /// </summary>
    /// <typeparam name = "T"></typeparam>
    /// <param name = "task"></param>
    public T GetResult<T>(Task<T> task)
    {
        T result = default;
        task.LogExceptions();
#region 1. ConfigureAwait(true)
        var configuredTask = task.Conf();
        var taskAwaiter = configuredTask.GetAwaiter();
        result = taskAwaiter.GetResult();
#endregion 1. ConfigureAwait(true)
#region 2. Sync
        //result = task.Result;
#endregion 2. Sync
#region 3. await
        //result = Await<T>(task);
#endregion 3. await
        return result;
    }

    public void GetResult(Task task)
    {
        task.LogExceptions();
        task.Conf();
    }

    public async Task RunAsync(Task task)
    {
        await task;
    }

    /// <summary>
    ///     Execute's an T> method which has a T return type synchronously
    /// </summary>
    public T RunSync<T, T1>(Func<T1, T> task, T1 a1)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        T ret = default;
        synch.Post(async _ =>
        {
            try
            {
                ret = task(a1);
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
        return ret;
    }

    /// <summary>
    ///     Execute's an T> method which has a T return type synchronously
    /// </summary>
    public T? RunSync<T, T1, T2>(Func<T1, T2, T> task, T1 a1, T2 a2)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        T? ret = default;
        synch.Post(_ =>
        {
            try
            {
                ret = task(a1, a2);
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
        return ret;
    }

    /// <summary>
    ///     Execute's an T> method which has a T return type synchronously
    /// </summary>
    /// <typeparam name = "T">Return Type</typeparam>
    /// <param name = "task">T> method to execute</param>
    public T RunSync<T, T1, T2, T3>(Func<T1, T2, T3, T> task, T1 a1, T2 a2, T3 a3)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        T ret = default;
        synch.Post(async _ =>
        {
            try
            {
                ret = task(a1, a2, a3);
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
        return ret;
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
                ci.GetResult(task());
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

    public void RunSyncWithoutReturnValue<T1>(Func<T1, Task> task, T1 a1)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(async _ =>
        {
            try
            {
                ci.GetResult(task(a1));
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

    public void RunSyncWithoutReturnValue<T1, T2>(Func<T1, T2, Task> task, T1 a1, T2 a2)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(async _ =>
        {
            try
            {
                ci.GetResult(task(a1, a2));
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