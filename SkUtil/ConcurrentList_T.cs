using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace SkUtil
{
    public class ConcurrentList<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IDisposable
    {
        private readonly List<T> _list;

        private readonly ReaderWriterLockSlim _lock;

        public int Count
        {
            get
            {
                int count;
                try
                {
                    this._lock.EnterReadLock();
                    count = this._list.Count;
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
                return count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public T this[int index]
        {
            get
            {
                T item;
                try
                {
                    this._lock.EnterReadLock();
                    item = this._list[index];
                }
                finally
                {
                    this._lock.ExitReadLock();
                }
                return item;
            }
            set
            {
                try
                {
                    this._lock.EnterWriteLock();
                    this._list[index] = value;
                }
                finally
                {
                    this._lock.ExitWriteLock();
                }
            }
        }

        public ConcurrentList()
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._list = new List<T>();
        }

        public ConcurrentList(int capacity)
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._list = new List<T>(capacity);
        }

        public ConcurrentList(IEnumerable<T> items)
        {
            this._lock = new ReaderWriterLockSlim(LockRecursionPolicy.NoRecursion);
            this._list = new List<T>(items);
        }

        public void Add(T item)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.Add(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public void Clear()
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.Clear();
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public bool Contains(T item)
        {
            bool flag;
            try
            {
                this._lock.EnterReadLock();
                flag = this._list.Contains(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
            return flag;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            try
            {
                this._lock.EnterReadLock();
                this._list.CopyTo(array, arrayIndex);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
        }

        public void Dispose()
        {
            this.Dispose(true);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
            this._lock.Dispose();
        }

        ~ConcurrentList()
        {
            this.Dispose(false);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ConcurrentEnumerator<T>(this._list, this._lock);
        }

        public int IndexOf(T item)
        {
            int num;
            try
            {
                this._lock.EnterReadLock();
                num = this._list.IndexOf(item);
            }
            finally
            {
                this._lock.ExitReadLock();
            }
            return num;
        }

        public void Insert(int index, T item)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.Insert(index, item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        public bool Remove(T item)
        {
            bool flag;
            try
            {
                this._lock.EnterWriteLock();
                flag = this._list.Remove(item);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
            return flag;
        }

        public void RemoveAt(int index)
        {
            try
            {
                this._lock.EnterWriteLock();
                this._list.RemoveAt(index);
            }
            finally
            {
                this._lock.ExitWriteLock();
            }
        }

        IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new ConcurrentEnumerator<T>(this._list, this._lock);
        }
    }
}
