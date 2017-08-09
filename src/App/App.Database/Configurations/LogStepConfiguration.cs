using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Entities;

namespace App.Database.Configurations
{
    public class LogStepConfiguration : EntityTypeConfiguration<LogStep>
    {


        public LogStepConfiguration() : this("dbo")
        {

        }

        private LogStepConfiguration(string schema)
        {
            ToTable(Tables.LogStepsTable, schema);

            HasKey(x => x.Id);

            Property(x => x.Index).HasColumnName("Index").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.StepTimestamp).HasColumnName("StepTimestamp").HasColumnType("datetime").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Type).HasColumnName("Type").HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Source).HasColumnName("Source").HasColumnType("nvarchar").HasMaxLength(1000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Name).HasColumnName("Name").HasColumnType("nvarchar").HasMaxLength(1000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Frame).HasColumnName("Frame").HasColumnType("nvarchar").HasMaxLength(1000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Message).HasColumnName("Message").HasColumnType("nvarchar").HasMaxLength(1000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Metadata).HasColumnName("Metadata").HasColumnType("ntext").HasMaxLength(2000000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}