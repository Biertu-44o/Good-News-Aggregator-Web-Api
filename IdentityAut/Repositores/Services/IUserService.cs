using System.Security.Claims;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;

namespace IServices.Services
{
    public interface IUserService
    {
        public Task<Boolean> RegistrationAsync
            (UserRegistrationDto model);

        public Task<User> GetUserByEmailAsync(String email);

        public Task<Boolean> LoginAsync
            (UserLoginDto model);

        public Task<List<Claim>> GetUserClaimsAsync
            (String Email);

        public Task<Boolean> IsUserExistAsync(String email);
    }
}
