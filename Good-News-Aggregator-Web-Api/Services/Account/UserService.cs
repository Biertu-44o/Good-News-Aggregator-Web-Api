using System.Security.Claims;
using AutoMapper;
using Core.DTOs.Account;
using Data.CQS.Commands;
using Data.CQS.Queries;
using Entities_Context.Entities.UserNews;
using IServices;
using IServices.Services;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using NUnit.Framework.Constraints;
using Serilog;

namespace Services.Account
{
    public sealed class UserService : IUserService
    {


        private readonly IMediator _mediator;

        private readonly IMapper _mapper;

        private readonly IUiThemeService _uiTheme;

        private readonly IRoleService _role;

        public UserService(
            IMediator mediator,
            IMapper mapper, 
            IUiThemeService uiTheme, 
            IRoleService role)
        {
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));

            _uiTheme = uiTheme ?? throw new ArgumentNullException(nameof(mapper));

            _role = role ?? throw new ArgumentNullException(nameof(mapper));

            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task<Boolean> IsUserExistAsync(String email)
        {
            if (!String.IsNullOrEmpty(email))
            {
                return await _mediator.Send(new IsUserExistByEmailQuery()
                {
                    Email = email
                });
            }

            throw new ArgumentException();
        }


        public async Task<Boolean> RegistrationAsync(UserRegistrationDto modelDto)
        {

            if(!await IsUserExistAsync(modelDto.Email)){

                User newUser = _mapper.Map<User>(modelDto);

                newUser.ThemeId =await _uiTheme.GetIdDefaultThemeAsync();
                
                newUser.Password = MakeHash(modelDto.Password);

                newUser.Created=DateTime.Now;

                PictureBase64EncoderDecoder encoder=new PictureBase64EncoderDecoder();
                
                newUser.ProfilePicture = await 
                    encoder.PictureEncoder(@"C:\\Users\\User\\Desktop\\ASP-Project\\ASProject\\IdentityAut\\IdentityAut\\wwwroot\\images\\defaultImage3.jpg");

                UserRole? role = await _role.GetDefaultRoleAsync();


                if (role is null)
                {
                    throw new ArgumentException("Can't create role");
                }

                await _mediator.Send(new CreateUserWithRoleCommand()
                {
                    Role=role,
                    User=newUser,
                });


                Log.Information("User {0} successfully registered.", modelDto.Email);

                return true;
            }

            return false;
        }

        #region PasswordHash
        static String MakeHash(String Password)
        {
            return BCrypt.Net.BCrypt.HashPassword(Password);
        }

        static Boolean CheckPassword(String Password,String PasswordHash)
        {
            return BCrypt.Net.BCrypt.Verify(Password,PasswordHash);
        }
        #endregion

        public async Task<Boolean> LoginAsync(UserLoginDto modelDto)
        {
            try
            {
                User? сheckUser = await GetUserByEmailAsync(modelDto.Email);

                if (CheckPassword(modelDto.Password, сheckUser.Password))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (NullReferenceException)
            {
                Log.Error("Failed login attempt {0}: user does not exist", modelDto.Email);
                
                return false;
            }

        }

        public async Task<List<Claim>> GetUserClaimsAsync(String email)
        {
            if (String.IsNullOrEmpty(email))
            {
                throw new ArgumentNullException(nameof(email));
            }

            User? user;
            try
            {
                user = await GetUserByEmailAsync(email);
            }
            catch (NullReferenceException)
            {
                Log.Error("Failed create user claim attempt {0}: user does not exist", email);
                throw;
            }

            var roles = (await _role.GetUserRolesByUserIdAsync(user.Id));

            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, user.Email),
            };

            if (roles.Count==0 || String.IsNullOrEmpty(roles.First().Role))
            {
                Log.Error("Failed create user claim attempt {0}: empty user roles", email);
                throw new NullReferenceException();
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Role));
            }

            return claims;
        }

        public async Task<User> GetUserByEmailAsync(String email)
        {
            try
            {
                return await _mediator.Send(new GetUserByEmailQuery()
                {
                    Email = email
                });
            }
            catch (NullReferenceException)
            {
                Log.Error("User {0} is not found", email);
                throw;
            }
           
        }
    }
}
