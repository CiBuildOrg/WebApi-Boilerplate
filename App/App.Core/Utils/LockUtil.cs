using System;
using System.Threading;

namespace App.Core.Utils
{
    public sealed class LockUtil : IDisposable
    {
        private readonly object _lockObj;

        public LockUtil(object lockObj) : this(lockObj, TimeSpan.FromSeconds(5)) { }

        private LockUtil(object lockObj, TimeSpan timeout)
        {
            _lockObj = lockObj;
            if (!Monitor.TryEnter(_lockObj, timeout))
                throw new TimeoutException();
        }

        public void Dispose()
        {
            Monitor.Exit(_lockObj);
        }
    }
}