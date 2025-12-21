namespace SunamoAsync;

// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
public partial class AsyncHelper
{
    public void RunSyncWithoutReturnValue<T1, T2, T3>(Func<T1, T2, T3, Task> task, T1 argument1, T2 argument2, T3 argument3)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(async _ =>
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
        private readonly Queue<Tuple<SendOrPostCallback, object>> items = new();
        private readonly AutoResetEvent workItemsWaiting = new(false);
        private bool done;
        public Exception? InnerException { get; set; }

        public void BeginMessageLoop()
        {
            while (!done)
            {
                Tuple<SendOrPostCallback, object> task = null;
                lock (items)
                {
                    if (items.Count > 0)
                        task = items.Dequeue();
                }

                if (task != null)
                {
                    task.Item1(task.Item2);
                    if (InnerException != null) // the method threw an exeption
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

        public override void Post(SendOrPostCallback callback, object state)
        {
            lock (items)
            {
                items.Enqueue(Tuple.Create(callback, state));
            }

            workItemsWaiting.Set();
        }

        public override void Send(SendOrPostCallback callback, object state)
        {
            throw new Exception("WeCannotSendToOurSameThread");
        }
    }
}