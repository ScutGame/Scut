using System.Collections.Generic;
using System.Threading;

namespace ZyGames.MSMQ.Model
{
    public sealed class SafedQueue<T>
    {
        #region private Fields
        private int isTaked = 0;
        private Queue<T> queue = new Queue<T>();
        private int MaxCount = 1000 * 1000;
        #endregion

        public int Count
        {
            get { return queue.Count; }
        }
        public void Enqueue(T t)
        {
            try
            {
                while (Interlocked.Exchange(ref isTaked, 1) != 0)
                {
                }
                this.queue.Enqueue(t);
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }

        public T Dequeue()
        {
            try
            {
                while (Interlocked.Exchange(ref isTaked, 1) != 0)
                {
                }
                T t = this.queue.Dequeue();
                return t;
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }

        public bool TryEnqueue(T t)
        {
            try
            {
                for (int i = 0; i < MaxCount; i++)
                {
                    if (Interlocked.Exchange(ref isTaked, 1) == 0)
                    {
                        this.queue.Enqueue(t);
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }

        public bool TryDequeue(out T t)
        {
            try
            {
                for (int i = 0; i < MaxCount; i++)
                {
                    if (Interlocked.Exchange(ref isTaked, 1) == 0)
                    {
                        t = this.queue.Dequeue();
                        return true;
                    }
                }
                t = default(T);
                return false;
            }
            finally
            {
                Thread.VolatileWrite(ref isTaked, 0);
            }
        }
    }
}
