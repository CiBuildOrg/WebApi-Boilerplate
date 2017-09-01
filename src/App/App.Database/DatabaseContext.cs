using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Extensions;
using App.Database.Implementations;
using App.Entities;
using App.Entities.Security;

namespace App.Database
{
    public class DatabaseContext : DbContextBase
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

        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

        public DbSet<Dummy> Dummy { get; set; }
        public DbSet<LogEntry> LogEntries { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}