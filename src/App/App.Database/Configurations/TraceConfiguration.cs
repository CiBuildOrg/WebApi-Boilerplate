using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Entities;

namespace App.Database.Configurations
{
    public class TraceConfiguration : EntityTypeConfiguration<Trace>
    {
        public const int MaxBodySize = 2000000;
        public TraceConfiguration() : this("dbo")
        {

        }

        private TraceConfiguration(string schema)
        {
            ToTable(Tables.TracesTable, schema);

            HasKey(x => x.Id);
            Property(x => x.CallerIdentity).HasColumnName("CallerIdentity").HasMaxLength(256).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RequestTimestamp).HasColumnName("RequestTimestamp").HasColumnType("datetime").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Verb).HasColumnName("Verb").HasMaxLength(16).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.Url).HasColumnName("Url").HasMaxLength(128).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RequestPayload).HasColumnName("RequestPayload").HasColumnType("ntext").HasMaxLength(MaxBodySize).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.StatusCode).HasColumnType("int").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ReasonPhrase).HasColumnName("ReasonPhrase").HasMaxLength(64).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.RequestHeaders).HasColumnName("RequestHeaders").HasMaxLength(4000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ResponseHeaders).HasColumnName("ResponseHeaders").HasMaxLength(4000).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.ResponsePayload).HasColumnName("ResponsePayload").HasColumnType("ntext").HasMaxLength(MaxBodySize).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
            Property(x => x.CallDuration).HasColumnName("CallDuration").HasMaxLength(32).IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}