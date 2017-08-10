using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Entities.Security;

namespace App.Database.Configurations
{
    public class ClientConfiguration : EntityTypeConfiguration<Client>
    {
        public ClientConfiguration()
            : this("dbo")
        {

        }

        private ClientConfiguration(string schema)
        {
            ToTable(Tables.ClientsTable, schema);

            HasKey(x => x.Id);

            Property(x => x.Secret).HasColumnName("Secret").HasColumnType("nvarchar").IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Name).HasMaxLength(100).HasColumnName("Name")
                .HasColumnType("nvarchar").IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.ApplicationType).HasColumnName("ApplicationType").HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.Active).HasColumnName("Active").HasColumnType("bit")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.RefreshTokenLifeTime).HasColumnName("RefreshTokenLifeTime").HasColumnType("int")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.AllowedOrigin).HasColumnName("AllowedOrigin").HasColumnType("nvarchar").HasMaxLength(100)
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasMany(x => x.RefreshTokens).WithRequired(x => x.Client).HasForeignKey(x => x.ClientId);
        }
    }
}
