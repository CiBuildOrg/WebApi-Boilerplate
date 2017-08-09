using System;
using System.Threading;

namespace App.Core.Utils
{
    public sealed class ConcurrentList<T> : ThreadSafeList<T>
    {
        // ReSharper disable once StaticMemberInGenericType

        private static readonly int[] Sizes;

        // ReSharper disable once StaticMemberInGenericType

        private static readonly int[] Counts;

        static ConcurrentList()
        {
            Sizes = new int[32];
            Counts = new int[32];

            var size = 1;
            var count = 1;
            for (var i = 0; i < Sizes.Length; i++)
            {
                Sizes[i] = size;
                Counts[i] = count;

                if (i < Sizes.Length - 1)
                {
                    size *= 2;
                    count += size;
                }
            }
        }

        private int _index;
        private int _fuzzyCount;
        private int _count;
        private readonly T[][] _array;

        public ConcurrentList()
        {
            _array = new T[32][];
        }

        public override T this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                {
                    throw new ArgumentOutOfRangeException(nameof(index));
                }

                var arrayIndex = GetArrayIndex(index + 1);
                if (arrayIndex > 0)
                {
                    index -= ((int)Math.Pow(2, arrayIndex) - 1);
                }

                return _array[arrayIndex][index];
            }
        }

        public override int Count => _count;

        public override void Add(T element)
        {
            var index = Interlocked.Increment(ref _index) - 1;
            var adjustedIndex = index;

            var arrayIndex = GetArrayIndex(index + 1);
            if (arrayIndex > 0)
            {
                adjustedIndex -= Counts[arrayIndex - 1];
            }

            if (_array[arrayIndex] == null)
            {
                var arrayLength = Sizes[arrayIndex];
                Interlocked.CompareExchange(ref _array[arrayIndex], new T[arrayLength], null);
            }

            _array[arrayIndex][adjustedIndex] = element;

            var count = _count;
            var fuzzyCount = Interlocked.Increment(ref _fuzzyCount);
            if (fuzzyCount == index + 1)
            {
                Interlocked.CompareExchange(ref _count, fuzzyCount, count);
            }
        }

        public override void CopyTo(T[] array, int index)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }

            var count = _count;
            if (array.Length - index < count)
            {
                throw new ArgumentException("There is not enough available space in the destination array.");
            }

            var arrayIndex = 0;
            var elementsRemaining = count;
            while (elementsRemaining > 0)
            {
                var source = _array[arrayIndex++];
                var elementsToCopy = Math.Min(source.Length, elementsRemaining);
                var startIndex = count - elementsRemaining;

                Array.Copy(source, 0, array, startIndex, elementsToCopy);

                elementsRemaining -= elementsToCopy;
            }
        }

        private static int GetArrayIndex(int count)
        {
            var arrayIndex = 0;

            if ((count & 0xFFFF0000) != 0)
            {
                count >>= 16;
                arrayIndex |= 16;
            }

            if ((count & 0xFF00) != 0)
            {
                count >>= 8;
                arrayIndex |= 8;
            }

            if ((count & 0xF0) != 0)
            {
                count >>= 4;
                arrayIndex |= 4;
            }

            if ((count & 0xC) != 0)
            {
                count >>= 2;
                arrayIndex |= 2;
            }

            if ((count & 0x2) != 0)
            {
                count >>= 1;
                arrayIndex |= 1;
            }

            return arrayIndex;
        }

        #region "Protected methods"

        protected override bool IsSynchronizedBase
        {
            get { return false; }
        }

        #endregion
    }
}