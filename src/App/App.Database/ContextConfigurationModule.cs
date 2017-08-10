using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Configurations;
using App.Entities.Security;

namespace App.Database
{
    public class ContextConfigurationModule : IConfigurationModule
    {
        private const string schema = "dbo";

        public void Register(DbModelBuilder modelBuilder)
        {
            // add entities configurations here
            modelBuilder.Configurations.Add(new DummyConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new ClientConfiguration());
            modelBuilder.Configurations.Add(new RefreshTokenConfiguration());

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(au => au.ProfileInfo).WithRequiredPrincipal();

            modelBuilder.Entity<ApplicationIdentityUserClaim>().ToTable(Tables.ApplicationIdentityUserClaimsTable, schema);
            modelBuilder.Entity<ApplicationIdentityUserLogin>().ToTable(Tables.ApplicationIdentityUserLoginsTable, schema);
            modelBuilder.Entity<ApplicationIdentityUserRole>().ToTable(Tables.ApplicationIdentityUserRolesTable, schema);
            modelBuilder.Entity<ApplicationRole>().ToTable(Tables.ApplicationRolesTable, schema);
            modelBuilder.Entity<ApplicationUser>().ToTable(Tables.ApplicationUsersTable, schema);
        }
    }
}