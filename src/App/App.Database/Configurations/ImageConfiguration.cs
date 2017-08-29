using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;
using App.Entities;

namespace App.Database.Configurations
{
    public class ImageConfiguration: EntityTypeConfiguration<Image>
    {
        public ImageConfiguration()
            : this("dbo")
        {
            
        }

        private ImageConfiguration(string schema)
        {
            ToTable(Tables.ImageTable, schema);

            HasKey(x => x.Id);


            Property(x => x.DateStoredUtc)
                .HasColumnName("DateStoredUtc")
                .HasColumnType("datetime")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.ImageSize)
                .HasColumnName("ImageSize")
                .HasColumnType("int")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.ImageType)
                .HasColumnName("ImageType")
                .HasColumnType("int")
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.FileName)
                .HasColumnName("FileName")
                .HasColumnType("nvarchar")
                .HasMaxLength(64)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);

            Property(x => x.MimeType)
                .HasColumnName("MimeType")
                .HasColumnType("nvarchar")
                .HasMaxLength(64)
                .IsRequired()
                .HasDatabaseGeneratedOption(DatabaseGeneratedOption.None);
        }
    }
}
