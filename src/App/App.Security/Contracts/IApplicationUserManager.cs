using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using App.Entities.Security;

namespace App.Security.Contracts
{
    public interface IApplicationUserManager : IDisposable
    {
        ApplicationUser FindApplciationUser(Guid applicationUserId);
    }
}
