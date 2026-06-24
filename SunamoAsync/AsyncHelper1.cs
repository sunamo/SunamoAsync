namespace SunamoAsync;

public partial class AsyncHelper
{
    public void RunSyncWithoutReturnValue<T1, T2, T3>(Func<T1, T2, T3, Task> task, T1 argument1, T2 argument2, T3 argument3)
    {
        var oldContext = SynchronizationContext.Current;
        var exclusiveContext = new ExclusiveSynchronizationContext();
        SynchronizationContext.SetSynchronizationContext(exclusiveContext);
        exclusiveContext.Post(_ =>
        {
            try
            {
                Instance.GetResult(task(argument1, argument2, argument3));
            }
            catch (Exception exception)
            {
                exclusiveContext.InnerException = exception;
                throw;
            }
            finally
            {
                exclusiveContext.EndMessageLoop();
            }
        }, null);
        exclusiveContext.BeginMessageLoop();
        SynchronizationContext.SetSynchronizationContext(oldContext);
        exclusiveContext.Dispose();
    }

    private class ExclusiveSynchronizationContext : SynchronizationContext, IDisposable
    {
        private readonly Queue<Tuple<SendOrPostCallback, object?>> callbackQueue = new();
        private readonly AutoResetEvent workItemsWaiting = new(false);
        private bool isDone;
        public Exception? InnerException { get; set; }

        public void BeginMessageLoop()
        {
            while (!isDone)
            {
                Tuple<SendOrPostCallback, object?>? workItem = null;
                lock (callbackQueue)
                {
                    if (callbackQueue.Count > 0)
                        workItem = callbackQueue.Dequeue();
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
            Post(_ => isDone = true, null);
        }

        public override void Post(SendOrPostCallback callback, object? state)
        {
            lock (callbackQueue)
            {
                callbackQueue.Enqueue(Tuple.Create(callback, state));
            }

            workItemsWaiting.Set();
        }

        public override void Send(SendOrPostCallback callback, object? state)
        {
            throw new Exception("WeCannotSendToOurSameThread");
        }
    }
}
