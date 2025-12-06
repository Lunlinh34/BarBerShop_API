using BarberShop.DTO;
using BarberShop.Unit;

namespace BarberShop.Service
{
    public class UserService : IUserSevice
    {

        private readonly UnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(UnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        public Task<UserDto> AuthenticateAsync(string username, string password)
        {
            throw new NotImplementedException();
        }
    }
}
