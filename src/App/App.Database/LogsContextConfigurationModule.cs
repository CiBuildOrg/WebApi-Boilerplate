using System.Data.Entity;
using App.Core.Contracts;
using App.Database.Configurations;
using App.Entities;

namespace App.Database
{
    public class LogsContextConfigurationModule : IConfigurationModule
    {
        public void Register(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TraceConfiguration());
            modelBuilder.Entity<Trace>().ToTable(Tables.TracesTable, Tables.Schema);
        }
    }
}