using System.Data.Entity;
using App.Entities;

namespace App.Core.Contracts
{
    public interface IDatabaseContext : IDbContext
    {
        IDbSet<Dummy> Dummy { get; }

        IDbSet<LogEntry> LogEntries { get; set; }
    }
}