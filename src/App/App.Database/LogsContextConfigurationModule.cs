using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Configurations;

namespace App.Database
{
    public class LogsContextConfigurationModule : IConfigurationModule
    {
        public void Register(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TraceConfiguration());
        }
    }
}