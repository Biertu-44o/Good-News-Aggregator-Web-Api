using System.Security.Claims;
using Core.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using MVC.ControllerFactory;
using MVC.Filters.Validation;
using MVC.Models.AccountModels;
using Serilog;

namespace MVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IServiceFactory _serviceFactory;
        
        public AccountController
            (IServiceFactory serviceFactory
            )
        {
            if (serviceFactory is null)
            {
                throw new NullReferenceException(nameof(serviceFactory));
            }
            _serviceFactory = serviceFactory;
        }


        public async Task<IActionResult> CheckUserLoginExist(String Email)
        {

            return Ok(await _serviceFactory
                .CreateIdentityService()
                .IsUserExistAsync(Email));

        }

        public async Task<IActionResult> CheckUserRegistrationExist(String Email)
        {
            return Ok(!await _serviceFactory
                .CreateIdentityService()
                .IsUserExistAsync(Email));

        }


        [HttpGet]
        public IActionResult Registration()
        {
            return View("Registration");
        }

        [HttpGet]
        public IActionResult Login()
        {

            if (HttpContext.Request.Query.ContainsKey("ReturnUrl"))
            {
                var url = new UserLoginViewModel()
                {
                    ReturnUrl = HttpContext.Request.Query["ReturnUrl"]
                };
                return View(url);
            }
            return View();
        }


        [HttpPost]
        [RegistrationValidationFilter]
        public async Task<IActionResult> Registration
            ([FromForm] UserRegistrationViewModel model)
        {
            if (await _serviceFactory
                    .CreateIdentityService()
                    .RegistrationAsync(
                    _serviceFactory
                        .CreateMapperService()
                        .Map<UserRegistrationDto>(model)))
            {
                return RedirectToAction("Login", "Account");
            }

            return Conflict("Пользователь уже существует");
        }



        [HttpPost]
        [ServiceFilter(typeof(LoginValidationFilterAttribute))]
        public async Task<IActionResult> Login([FromForm] UserLoginViewModel model)
        {

            const string authType = "Application Cookie";
            

            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, model.Email)
                };


            var roles = (await _serviceFactory
                .CreateRoleService()
                .GetUserRolesByUserNameAsync(model.Email));


            if (roles is null)
            {
                throw new ArgumentException("Incorrect user or role", nameof(model));
            }

            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role.Role));
            }

            var identity = new ClaimsIdentity(claims,
                authType,
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);


            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));
            
            Log.Information("User {0} ip:{1} went to the website", model.Email
                , HttpContext.Connection.RemoteIpAddress?.ToString());
            
            if (!string.IsNullOrEmpty(model.ReturnUrl) && Url.IsLocalUrl(model.ReturnUrl))
            {
                return LocalRedirect(model.ReturnUrl);
            }

            return RedirectToAction("GetInfoConfig", "Settings");
        }


        public async Task<IActionResult> LogOut()
        {
            await _serviceFactory.CreateIdentityService().IdLogoutAsync();
            return RedirectToAction("GetArticlesByPage", "Article");
        }

    }
}