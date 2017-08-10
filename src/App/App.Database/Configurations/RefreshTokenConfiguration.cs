using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Entities.Security;

namespace App.Database.Configurations
{
    public class RefreshTokenConfiguration : EntityTypeConfiguration<RefreshToken>
    {
        public RefreshTokenConfiguration() : this("dbo") { }

        private RefreshTokenConfiguration(string schema)
        {
            ToTable(Tables.RefreshTokensTable, schema);

            HasKey(x => x.Id);

            Property(x => x.Subject).HasColumnType("nvarchar").HasColumnName("Subject")
                .IsRequired().HasMaxLength(50).HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            //Property(x => x.ClientId).HasColumnType("uniqueidentifier").HasColumnName("ClientId")
            //    .IsRequired().HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.ProtectedTicket).HasColumnType("nvarchar").HasColumnName("ProtectedTicket")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.IssuedUtc).HasColumnName("IssuedUtc").HasColumnType("datetime")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.ExpiresUtc).HasColumnName("ExpiresUtc").HasColumnType("datetime")
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            HasRequired(x => x.User).WithOptional(x => x.RefreshToken);
        }
    }
}