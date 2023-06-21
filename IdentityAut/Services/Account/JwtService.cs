﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Azure.Core;
using Core.DTOs.Account;
using Data.CQS.Commands;
using Data.CQS.Queries;
using Entities_Context.Data.Migration.UserArticle;
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

            if (SecurityKey.IsNullOrEmpty())
            {
                throw new InvalidOperationException("Can't read configuration file");
            }

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration["Jwt:SecurityKey"]!));

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
                    Log.Error("Can't find user:" + nameof(email));
                    throw new NullReferenceException();
                }


            }
            catch (ArgumentException)
            {
                Log.Error("RT not connected with User", nameof(refreshToken));
                throw new ArgumentException();
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
                    Log.Warning("Can't get user claims: {0} ", nameof(user));
                    throw new NullReferenceException();
                }
            }
            else
            {
                Log.Warning("User with token not found: {0} ", nameof(refreshToken));

                throw new NullReferenceException();
            }

        }
    }
}
