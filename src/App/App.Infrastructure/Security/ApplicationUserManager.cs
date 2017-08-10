using System;
using App.Database;
using App.Entities.Security;
using App.Infrastructure.Contracts;

namespace App.Infrastructure.Security
{
    public class ApplicationUserManager : IApplicationUserManager
    {
        private readonly DatabaseContext _context;

        public ApplicationUserManager(DatabaseContext context)
        {
            _context = context;
        }
        public ApplicationUser FindApplciationUser(Guid applicationUserId)
        {
            return _context.Users.Find(applicationUserId);
        }

        private bool _disposed;
        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                // dispose managed resources
                _context.Dispose();
            }

            _disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        
    }
}