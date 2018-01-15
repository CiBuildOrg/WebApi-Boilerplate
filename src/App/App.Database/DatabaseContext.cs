using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Extensions;
using App.Database.Implementations;
using App.Entities;
using App.Entities.Security;
using App.Core;
using System.Data.Common;

namespace App.Database
{
    public class DatabaseContext : IdentityDbBase
    {   
        public DatabaseContext(IConfiguration configuration)
            : base(ConfigurationKeys.DatabaseConnectionString, configuration, new ContextConfigurationModule())
        {
            this.DisableDatabaseInitialization();
        }

        // only used in development
        internal DatabaseContext(string connectionString)
            : base(connectionString, new ContextConfigurationModule())
        {
            this.DisableDatabaseInitialization();
        }

        public DatabaseContext(DbConnection connection) : base(connection, new ContextConfigurationModule())
        {
            this.DisableDatabaseInitialization();
        }

        public virtual DbSet<UserProfile> UserProfiles { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<Dummy> Dummy { get; set; }
        public DbSet<Image> Images { get; set; }
    }
}