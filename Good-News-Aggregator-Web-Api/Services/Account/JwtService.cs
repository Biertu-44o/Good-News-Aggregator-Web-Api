using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.DTOs.Account;
using Data.CQS.Commands;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using IServices.Services;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using Serilog;

namespace Services.Account
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;
        private readonly IUserService _user;

        public JwtService(IConfiguration configuration, IMediator mediator, IUserService user)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            _user = user ?? throw new ArgumentNullException(nameof(user));
        }

        public async Task<String> GetJwtTokenString(List<Claim> claims)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("D")));
            claims.Add(new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString("R")));

            var SecurityKey = _configuration["Jwt:SecurityKey"];

            if (String.IsNullOrEmpty(SecurityKey))
            {
                throw new InvalidOperationException("Can't read configuration file");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                SecurityKey!));

            var signIn = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);


            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:ExpireInMinutes"])),
                signingCredentials: signIn);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task AddRefreshTokenAsync(String email, Guid refreshToken)
        {
            try
            {
                User? user = await _user.GetUserByEmailAsync(email);

                if (user != null)
                {
                    await _mediator.Send(new AddRefreshTokenCommand()
                    {
                        Id = user.Id,
                        Value = refreshToken
                    });
                }
                else
                {
                    throw new NullReferenceException("Can't find user:" + nameof(email));
                }


            }
            catch (ArgumentException)
            {
                throw new ArgumentException("RT not connected with User", nameof(refreshToken));
            }
        }

        public async Task<TokenDto> RefreshTokenAsync(Guid refreshToken)
        {
            
            User? user = await _mediator.Send(new GetUserByRefreshTokenQuery()
                {
                    RefreshToken = refreshToken
                });

            if (user != null)
            {
                var userClaims = await _user.GetUserClaimsAsync(user.Email);

                if (userClaims.Count!=0)
                {
                    var jwt = await GetJwtTokenString(userClaims);

                    await _mediator.Send(new RemoveRefreshTokenCommand() { RefreshToken = refreshToken });

                    var newRT = Guid.NewGuid();

                    await AddRefreshTokenAsync(user.Email, newRT);

                    return new TokenDto()
                    {
                        JwtToken = jwt,
                        RefreshToken = newRT.ToString("D")
                    };
                }
                else
                {
                    throw new NullReferenceException("Can't get user claims: " + nameof(user));
                }
            }
            else
            {
                throw new NullReferenceException("User with token not found: " + nameof(refreshToken));
            }

        }
    }
}
