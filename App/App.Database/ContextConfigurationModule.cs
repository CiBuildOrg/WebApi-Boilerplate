using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Configurations;
using App.Database.Security;
using App.Entities;

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
        }
    }
}