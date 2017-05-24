using System;
using System.Collections;
using System.Collections.Generic;

namespace UmbracoVault.Collections
{
    public abstract class ReadOnlySet<T> : ISet<T>
    {
        protected readonly ISet<T> BackingSet;

        protected ReadOnlySet(ISet<T> backingSet)
        {
            BackingSet = backingSet;
        }

        public int Count => BackingSet.Count;

        public bool IsReadOnly { get; } = true;

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return BackingSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return BackingSet.IsSupersetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return BackingSet.IsProperSupersetOf(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return BackingSet.IsProperSubsetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return BackingSet.Overlaps(other);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return BackingSet.SetEquals(other);
        }

        public bool Contains(T item)
        {
            return BackingSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            BackingSet.CopyTo(array, arrayIndex);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return BackingSet.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return BackingSet.GetEnumerator();
        }

        public bool Add(T item)
        {
            throw new NotImplementedException("ReadOnlySet does not support adding elements.");
        }

        public void Clear()
        {
            throw new NotImplementedException("ReadOnlySet does not support clearing elements.");
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException("ReadOnlySet does not support except with operation.");
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            throw new NotImplementedException("ReadOnlySet does not support intersect with operation.");
        }
        public bool Remove(T item)
        {
            throw new NotImplementedException("ReadOnlySet does not support removing elements.");
        }
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            throw new NotImplementedException("ReadOnlySet does not support symmetric except with operation.");
        }
        public void UnionWith(IEnumerable<T> other)
        {
            throw new NotImplementedException("ReadOnlySet does not support union with operation.");
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException("ReadOnlySet does not support adding elements.");
        }
    }
}