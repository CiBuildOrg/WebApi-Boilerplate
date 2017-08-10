using System;
using App.Entities.Security;

namespace App.Infrastructure.Contracts
{
    public interface IApplicationUserManager : IDisposable
    {
        ApplicationUser FindApplciationUser(Guid applicationUserId);
    }
}
