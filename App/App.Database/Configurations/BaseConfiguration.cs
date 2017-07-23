using System.Data.Entity.ModelConfiguration;
using App.Entities;

namespace App.Database.Configurations
{
    public class BaseConfiguration<T> : EntityTypeConfiguration<T> where T : BaseEntity
    {
        protected BaseConfiguration()
        {
            HasKey(x => x.Id);
        }
    }
}
