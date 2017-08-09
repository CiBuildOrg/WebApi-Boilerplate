using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using App.Database;
using App.Dto.Request;
using App.Entities.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace App.Api.Security
{
    public class RefreshTokenManager : IRefreshTokenManager
    {
        private readonly DatabaseContext _context;

        public RefreshTokenManager(DatabaseContext dbContext)
        {
            _context = dbContext;
        }

        public IEnumerable<Client> GetClients()
        {
            return _context.Clients.ToList();
        }

        public IEnumerable<Client> GetAllowedClients()
        {
            return _context.Clients.Where(x => x.Active);
        }

        public Client FindClient(string clientId)
        {
            var client = _context.Clients.Find(Guid.Parse(clientId));
            return client;
        }

        public async Task<Client> AddClientAsync(ClientBindingModel clientModel)
        {
            var key = new byte[32];
            RandomNumberGenerator.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);

            var client = new Client
            {
                Id = Guid.NewGuid(),
                Active = true,
                AllowedOrigin = string.IsNullOrWhiteSpace(clientModel.AllowedOrigin) ? "*" : clientModel.AllowedOrigin,
                ApplicationType = clientModel.ApplicationType,
                Name = clientModel.Name,
                RefreshTokenLifeTime = clientModel.RefreshTokenLifeTime,
                Secret = base64Secret
            };

            _context.Clients.Add(client);

            var result = await _context.SaveChangesAsync() > 0;
            return result ? client : null;
        }

        public async Task<bool> RemoveClient(string id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client == null)
            {
                return false;
            }

            _context.Clients.Remove(client);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRefreshToken(RefreshToken token)
        {
            var existingToken = _context.RefreshTokens.SingleOrDefault(r => r.Subject == token.Subject && r.ClientId == token.ClientId);
            if (existingToken != null)
            {
                await RemoveRefreshToken(existingToken);
            }

            _context.RefreshTokens.Add(token);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(RefreshToken existingToken)
        {
            _context.RefreshTokens.Remove(existingToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveRefreshToken(string refreshTokenId)
        {
            var refreshToken = _context.RefreshTokens.Find(refreshTokenId);
            if (refreshToken == null)
            {
                return false;
            }

            _context.RefreshTokens.Remove(refreshToken);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<RefreshToken> FindRefreshToken(string refreshTokenId)
        {
            var refreshToken = await _context.RefreshTokens.FindAsync(refreshTokenId);
            return refreshToken;
        }

        public List<RefreshToken> GetAllRefreshTokens()
        {
            return _context.RefreshTokens.ToList();
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