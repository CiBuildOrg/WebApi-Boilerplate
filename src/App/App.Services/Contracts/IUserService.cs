using App.Dto.Request;

namespace App.Services.Contracts
{
    public interface IUserService
    {
        void Register(NewUserDto request);
    }
}