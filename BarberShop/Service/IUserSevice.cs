using BarberShop.DTO;

namespace BarberShop.Service
{
    public interface IUserSevice
    {
        Task<UserDto> AuthenticateAsync(string username, string password);
    }
}
