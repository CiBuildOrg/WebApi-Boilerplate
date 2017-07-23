using System.ComponentModel.DataAnnotations.Schema;
using App.Entities;

namespace App.Database.Configurations
{
    public class DummyConfiguration : BaseConfiguration<Dummy>
    {
        public DummyConfiguration()
            : this("dbo")
        {

        }

        private DummyConfiguration(string schema)
        {
            ToTable(Tables.DummyTable, schema);
            Property(x => x.DummyData)
                .HasColumnName("DummyData")
                .HasColumnType("nvarchar")
                .HasMaxLength(2000)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}