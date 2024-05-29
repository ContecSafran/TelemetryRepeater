using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
namespace SkUtil
{
    public class ConcurrentEnumerator<T> : IEnumerator<T>, IDisposable, IEnumerator
    {
        private readonly IEnumerator<T> _inner;

        private readonly ReaderWriterLockSlim _lock;

        public T Current
        {
            get
            {
                return this._inner.Current;
            }
        }

        object System.Collections.IEnumerator.Current
        {
            get
            {
                return this._inner.Current;
            }
        }

        public ConcurrentEnumerator(IEnumerable<T> inner, ReaderWriterLockSlim @lock)
        {
            this._lock = @lock;
            this._lock.EnterReadLock();
            this._inner = inner.GetEnumerator();
        }

        public void Dispose()
        {
            this._lock.ExitReadLock();
        }

        public bool MoveNext()
        {
            return this._inner.MoveNext();
        }

        public void Reset()
        {
            this._inner.Reset();
        }
    }
}
