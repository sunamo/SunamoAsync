// variables names: ok
namespace SunamoAsync;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class AsyncHelper
{
    /// <summary>
    ///     Execute's an async method with three parameters synchronously without return value
    /// </summary>
    /// <typeparam name = "T1">First parameter type</typeparam>
    /// <typeparam name = "T2">Second parameter type</typeparam>
    /// <typeparam name = "T3">Third parameter type</typeparam>
    /// <param name = "task">Async method to execute</param>
    /// <param name = "argument1">First argument</param>
    /// <param name = "argument2">Second argument</param>
    /// <param name = "argument3">Third argument</param>
    public void RunSyncWithoutReturnValue<T1, T2, T3>(Func<T1, T2, T3, Task> task, T1 argument1, T2 argument2, T3 argument3)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(_ =>
        {
            try
            {
                Instance.GetResult(task(argument1, argument2, argument3));
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

    private class ExclusiveSynchronizationContext : SynchronizationContext, IDisposable
    {
        private readonly Queue<Tuple<SendOrPostCallback, object?>> items = new();
        private readonly AutoResetEvent workItemsWaiting = new(false);
        private bool done;
        public Exception? InnerException { get; set; }

        public void BeginMessageLoop()
        {
            while (!done)
            {
                Tuple<SendOrPostCallback, object?>? workItem = null;
                lock (items)
                {
                    if (items.Count > 0)
                        workItem = items.Dequeue();
                }

                if (workItem != null)
                {
                    workItem.Item1(workItem.Item2);
                    if (InnerException != null) // the method threw an exception
                        throw new Exception("AsyncHelpersRunMethodThrewAnException" + ". " + InnerException);
                }
                else
                {
                    workItemsWaiting.WaitOne();
                }
            }
        }

        public override SynchronizationContext CreateCopy()
        {
            return this;
        }

        public void Dispose()
        {
            workItemsWaiting.Dispose();
        }

        public void EndMessageLoop()
        {
            Post(_ => done = true, null);
        }

        public override void Post(SendOrPostCallback callback, object? state)
        {
            lock (items)
            {
                items.Enqueue(Tuple.Create(callback, state));
            }

            workItemsWaiting.Set();
        }

        public override void Send(SendOrPostCallback callback, object? state)
        {
            throw new Exception("WeCannotSendToOurSameThread");
        }
    }
}