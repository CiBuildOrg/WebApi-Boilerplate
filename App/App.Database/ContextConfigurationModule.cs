using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Configurations;

namespace App.Database
{
    public class ContextConfigurationModule : IConfigurationModule
    {
        public void Register(DbModelBuilder modelBuilder)
        {
            // add entities configurations here
            modelBuilder.Configurations.Add(new DummyConfiguration());
        }
    }
}