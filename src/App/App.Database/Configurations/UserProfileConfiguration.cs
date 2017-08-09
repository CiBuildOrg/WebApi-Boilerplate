using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Entities;

namespace App.Database.Configurations
{
    public class UserProfileConfiguration : EntityTypeConfiguration<UserProfile>
    {
        public UserProfileConfiguration()
            : this("dbo")
        {
            
        }

        private UserProfileConfiguration(string schema)
        {
            ToTable(Tables.UserProfileTable, schema);

            HasKey(x => x.Id);

            Property(x => x.Description)
                .HasColumnName("Description")
                .HasColumnType("nvarchar")
                .HasMaxLength(2000)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.FullName)
                .HasColumnName("FullName")
                .HasColumnType("nvarchar")
                .HasMaxLength(2000)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.JoinDate)
                .HasColumnName("JoinDate")
                .HasColumnType("datetime")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}