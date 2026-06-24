namespace SunamoAsync;

// variables names: ok
public partial class AsyncHelper
{
    public static AsyncHelper Instance = new();
    private AsyncHelper()
    {
    }

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

    public T GetResult<T>(Task<T> task)
    {
        task.LogExceptions();
        var configuredTask = task.Conf();
        var taskAwaiter = configuredTask.GetAwaiter();
        return taskAwaiter.GetResult();
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
