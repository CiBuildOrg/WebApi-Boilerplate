using System.Data.Entity;

namespace App.Core.Contracts
{
    public interface IConfigurationModule
    {
        void Register(DbModelBuilder modelBuilder);
    }
}