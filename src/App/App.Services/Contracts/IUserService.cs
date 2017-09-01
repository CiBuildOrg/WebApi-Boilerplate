using App.Dto.Request;
using App.Dto.Response;

namespace App.Services.Contracts
{
    public interface IUserService
    {
        NewUserResponse Register(NewUserDto request);
    }
}