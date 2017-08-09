using System;
using App.Core.Contracts;

namespace App.Core.Implementations
{
    public class NowImplementation : INow
    {
        public DateTime Now => DateTime.Now;

        public DateTime UtcNow => DateTime.UtcNow;
    }
}
