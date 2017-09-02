using App.Dto.Request;
using App.Dto.Response;

namespace App.Services.Contracts
{
    public interface IUserService
    {
        RegistrationResult Register(NewUserDto request);
        bool UsernameAlreadyRegistered(string username);
        bool EmailAlreadyRegistered(string email);
    }
}