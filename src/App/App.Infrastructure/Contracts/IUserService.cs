using App.Dto.Request;
using App.Dto.Response;

namespace App.Infrastructure.Contracts
{
    public interface IUserService
    {
        NewUserResponse Register(NewUserDto request);
    }
}