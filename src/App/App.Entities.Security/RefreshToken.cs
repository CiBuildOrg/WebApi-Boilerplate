using System;

namespace App.Entities.Security
{
    public class RefreshToken
    {
        public string Id { get; set; }
        public string Subject { get; set; }
        public Guid ClientId { get; set; }
        public virtual Client Client { get; set; }
        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }
        public string ProtectedTicket { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}