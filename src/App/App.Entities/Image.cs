using System;
using App.Entities.Contracts;

namespace App.Entities
{
    public class Image : IBaseEntity
    {
        public Guid Id { get; set; }
        public Guid UserProfileId { get; set; }

        public ImageType ImageType { get; set; }
        public ImageSize ImageSize { get; set; }
        public string FileName { get; set; }
        public string MimeType { get; set; }

        public DateTime DateStoredUtc { get; set; }

        public virtual UserProfile UserProfile { get; set; }
    }
}