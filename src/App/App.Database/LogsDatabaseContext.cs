using App.Core.Contracts;
using App.Database.Extensions;
using App.Database.Implementations;
using App.Entities;
using System.Data.Entity;

namespace App.Database
{
    public class LogsDatabaseContext: DbContextBase
    {
        public LogsDatabaseContext(IConfiguration configuration)
            : base(configuration, new LogsContextConfigurationModule())
        {
            this.DisableDatabaseInitialization();
        }

        // only used in development
        internal LogsDatabaseContext(string connectionString)
            : base(connectionString, new LogsContextConfigurationModule())
        {
            this.DisableDatabaseInitialization();
        }

        public virtual DbSet<Trace> Traces { get; set; }
    }
}