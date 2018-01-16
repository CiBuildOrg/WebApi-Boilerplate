using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using App.Dto.Request;
using App.Entities.Security;

namespace App.Infrastructure.Contracts
{
    public interface IRefreshTokenManager : IDisposable
    {
        IEnumerable<Client> GetClients();
        IEnumerable<Client> GetAllowedClients();
        Client FindClient(Guid clientId);
        Task<Client> AddClientAsync(ClientBindingModel clientModel);
        Task<bool> RemoveClient(Guid id);
        Task<bool> AddRefreshToken(RefreshToken token);
        Task<bool> RemoveRefreshToken(RefreshToken existingToken);
        Task<bool> RemoveRefreshToken(string refreshTokenId);
        Task<RefreshToken> FindRefreshToken(string refreshTokenId);
        List<RefreshToken> GetAllRefreshTokens();
        Task<List<RefreshToken>> GetAllRefreshTokensAsync();
    }
}