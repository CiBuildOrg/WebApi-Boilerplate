using System;
using App.Entities.Contracts;

namespace App.Entities.Security
{
    public class RefreshToken 
    {
        public Guid Id { get; set; }

        public string RefreshTokenId { get; set; }

        public string Subject { get; set; }

        public string ClientId { get; set; }

        public DateTime IssuedUtc { get; set; }
        public DateTime ExpiresUtc { get; set; }

        public string ProtectedTicket { get; set; }
    }
}