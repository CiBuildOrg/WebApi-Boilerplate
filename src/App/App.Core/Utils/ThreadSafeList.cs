using System;
using System.Collections;
using System.Collections.Generic;

namespace App.Core.Utils
{
    public abstract class ThreadSafeList<T> : IList<T>, IList
    {
        public abstract T this[int index] { get; }

        public abstract int Count { get; }

        public abstract void Add(T item);

        public virtual int IndexOf(T item)
        {
            IEqualityComparer<T> comparer = EqualityComparer<T>.Default;

            var count = Count;
            for (var i = 0; i < count; i++)
            {
                if (comparer.Equals(item, this[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public virtual bool Contains(T item)
        {
            return IndexOf(item) != -1;
        }

        public abstract void CopyTo(T[] array, int arrayIndex);

        public IEnumerator<T> GetEnumerator()
        {
            var count = Count;
            for (var i = 0; i < count; i++)
            {
                yield return this[i];
            }
        }

        #region "Protected methods"

        protected abstract bool IsSynchronizedBase { get; }

        protected virtual void CopyToBase(Array array, int arrayIndex)
        {
            for (var i = 0; i < Count; ++i)
            {
                array.SetValue(this[i], arrayIndex + i);
            }
        }

        protected virtual int AddBase(object value)
        {
            Add((T)value);
            return Count - 1;
        }

        #endregion

        #region "Explicit interface implementations"

        T IList<T>.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.IsReadOnly => false;

        void ICollection<T>.Clear()
        {
            throw new NotSupportedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotSupportedException();
        }

        bool IList.IsFixedSize => false;

        bool IList.IsReadOnly => false;

        object IList.this[int index]
        {
            get => this[index];
            set => throw new NotSupportedException();
        }

        void IList.RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        void IList.Remove(object value)
        {
            throw new NotSupportedException();
        }

        void IList.Insert(int index, object value)
        {
            throw new NotSupportedException();
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        void IList.Clear()
        {
            throw new NotSupportedException();
        }

        bool IList.Contains(object value)
        {
            return ((IList)this).IndexOf(value) != -1;
        }

        int IList.Add(object value)
        {
            return AddBase(value);
        }

        bool ICollection.IsSynchronized => IsSynchronizedBase;

        object ICollection.SyncRoot => null;

        void ICollection.CopyTo(Array array, int arrayIndex)
        {
            CopyToBase(array, arrayIndex);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion
    }
}