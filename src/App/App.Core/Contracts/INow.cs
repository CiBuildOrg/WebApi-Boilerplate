using System;

namespace App.Core.Contracts
{
    public interface INow
    {
        DateTime Now { get; }
        DateTime UtcNow { get; }
    }
}