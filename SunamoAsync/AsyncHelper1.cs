// EN: Variable names have been checked and replaced with self-descriptive names
// CZ: Názvy proměnných byly zkontrolovány a nahrazeny samopopisnými názvy
namespace SunamoAsync;
public partial class AsyncHelper
{
    public void RunSyncWithoutReturnValue<T1, T2, T3>(Func<T1, T2, T3, Task> task, T1 a1, T2 a2, T3 a3)
    {
        var oldContext = SynchronizationContext.Current;
        var synch = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(synch);
        synch.Post(async _ =>
        {
            try
            {
                ci.GetResult(task(a1, a2, a3));
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

    private async Task<T> Await<T>(Task<T> taskToAwait)
    {
        return await taskToAwait;
    }

    private async Task Await(Task taskToAwait)
    {
        await taskToAwait;
    }

    private class ExclusiveSynchronizationContext : SynchronizationContext, IDisposable
    {
        private static Type type = typeof(ExclusiveSynchronizationContext);
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

        public override void Post(SendOrPostCallback d, object state)
        {
            lock (items)
            {
                items.Enqueue(Tuple.Create(d, state));
            }

            workItemsWaiting.Set();
        }

        public override void Send(SendOrPostCallback d, object state)
        {
            throw new Exception("WeCannotSendToOurSameThread");
        }
    }
}