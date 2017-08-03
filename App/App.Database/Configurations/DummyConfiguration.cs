using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Entities;

namespace App.Database.Configurations
{
    public class DummyConfiguration : EntityTypeConfiguration<Dummy>
    {
        public DummyConfiguration()
            : this("dbo")
        {

        }

        private DummyConfiguration(string schema)
        {
            ToTable(Tables.DummyTable, schema);

            HasKey(x => x.Id);

            Property(x => x.DummyData)
                .HasColumnName("DummyData")
                .HasColumnType("nvarchar")
                .HasMaxLength(2000)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}