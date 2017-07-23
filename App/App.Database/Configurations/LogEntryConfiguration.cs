using System.ComponentModel.DataAnnotations.Schema;
using App.Entities;

namespace App.Database.Configurations
{
    public class LogEntryConfiguration : BaseConfiguration<LogEntry>
    {

        public LogEntryConfiguration() : this("dbo")
        {

        }

        private LogEntryConfiguration(string schema)
        {
            ToTable(Tables.LogEntriesTable, schema);

            HasKey(x => x.Id);

            Property(x => x.Timestamp).HasColumnName("Timestamp").HasColumnType("datetime").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RequestTimestamp).HasColumnName("RequestTimestamp").HasColumnType("datetime").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ResponseTimestamp).HasColumnName("ResponseTimestamp").HasColumnType("datetime").HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RequestUri).HasColumnName("RequestUri").HasColumnType("nvarchar").HasMaxLength(256).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            HasMany(x => x.Steps).WithRequired(x => x.LogEntry).HasForeignKey(x => x.LogEntryId);
        }
    }
}