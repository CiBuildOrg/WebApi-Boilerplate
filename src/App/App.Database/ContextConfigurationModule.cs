using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Configurations;
using App.Entities.Security;

namespace App.Database
{
    public class ContextConfigurationModule : IConfigurationModule
    {
        public void Register(DbModelBuilder modelBuilder)
        {
            // add entities configurations here
            modelBuilder.Configurations.Add(new DummyConfiguration());
            modelBuilder.Configurations.Add(new UserProfileConfiguration());
            modelBuilder.Configurations.Add(new ClientConfiguration());
            modelBuilder.Configurations.Add(new RefreshTokenConfiguration());

            modelBuilder.Entity<ApplicationUser>()
                .HasRequired(au => au.ProfileInfo).WithRequiredPrincipal();
            modelBuilder.Entity<ApplicationIdentityUserClaim>().ToTable(Tables.ApplicationIdentityUserClaimsTable, Tables.Schema);
            modelBuilder.Entity<ApplicationIdentityUserLogin>().ToTable(Tables.ApplicationIdentityUserLoginsTable, Tables.Schema);
            modelBuilder.Entity<ApplicationIdentityUserRole>().ToTable(Tables.ApplicationIdentityUserRolesTable, Tables.Schema);
            modelBuilder.Entity<ApplicationRole>().ToTable(Tables.ApplicationRolesTable, Tables.Schema);
            modelBuilder.Entity<ApplicationUser>().ToTable(Tables.ApplicationUsersTable, Tables.Schema);
        }
    }
}