namespace Pinger.Services
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;

    public class RingBuffer<T>
    {
        private readonly ConcurrentQueue<T> queue;

        private readonly object objectLock = new object();

        public RingBuffer(int limit)
        {
            this.queue = new ConcurrentQueue<T>();
            this.Limit = limit;
        }

        public int Limit { get; }

        public IEnumerable<T> Get
        {
            get
            {
                lock (this.objectLock)
                {
                    return this.queue.ToArray();
                }
            }
        }

        public void AddItemToQueue(T item)
        {
            lock (objectLock)
            {
                if (queue.Count == this.Limit)
                {
                    this.queue.TryDequeue(out _);
                }

                this.queue.Enqueue(item);
            }
        }
    }
}