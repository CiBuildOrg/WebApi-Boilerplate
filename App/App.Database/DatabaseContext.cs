using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Extensions;
using App.Database.Implementations;
using App.Entities;

namespace App.Database
{
    public class DatabaseContext : DbContextBase, IDatabaseContext
    {
        public DatabaseContext(IConfiguration configuration)
            : base(configuration, new ContextConfigurationModule())
        {
            this.DisableDatabaseInitialization();
        }

        // only used in development
        internal DatabaseContext(string connectionString)
            : base(connectionString, new ContextConfigurationModule())
        {
            this.DisableDatabaseInitialization();
        }

        public IDbSet<Dummy> Dummy { get; set; }

        public IDbSet<LogEntry> LogEntries { get; set; }
    }
}