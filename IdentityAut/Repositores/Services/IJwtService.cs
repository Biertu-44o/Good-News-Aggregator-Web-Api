using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.DTOs.Account;
using Entities_Context.Entities.UserNews;

namespace IServices.Services
{
    public interface IJwtService
    {
        public Task<String> GetJwtTokenString(List<Claim> user);
        public Task AddRefreshTokenAsync(String email,Guid refreshToken);
        public Task<TokenDto> RefreshTokenAsync(Guid refreshToken);
    }
}
