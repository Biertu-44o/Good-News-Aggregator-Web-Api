using FluentValidation;
using IServices.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Core.DTOs.Account;
using FluentValidation.Results;
using Microsoft.IdentityModel.Tokens;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;
using Web_Api_Controllers.Validators;
using Web_Api_Controllers.ControllerFactory;

namespace Web_Api_Controllers.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public AuthController(
            IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }

        /// <summary>
        /// Login
        /// </summary>
        /// <param name="loginRequest">Login model. Password format: @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{5,16}$"</param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/login
        ///     {
        ///        "email": "exmple@email.ru",
        ///        "password": "Qwer123!!",
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successful login. Return Jwt and refresh token.</response>
        /// <response code="400">Bad valid or ( incorrect email or password ).</response>
        [ProducesResponseType(typeof(JwtTokenResponse),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {

            ValidationResult result = await _serviceFactory
                .CreateLoginValidator()
                .ValidateAsync(loginRequest);


            if (result.IsValid)
            {

                if (await _serviceFactory
                        .CreateIdentityService()
                        .LoginAsync(_serviceFactory
                            .CreateMapperService()
                            .Map<UserLoginDto>(loginRequest)))
                {
                    var userClaims = await _serviceFactory
                        .CreateIdentityService()
                        .GetUserClaimsAsync(loginRequest.Email);

                    var jwtTokenString = await _serviceFactory
                        .CreateJwtService()
                        .GetJwtTokenString(userClaims);

                    var refreshToken = Guid.NewGuid();

                    await _serviceFactory
                        .CreateJwtService()
                        .AddRefreshTokenAsync(loginRequest.Email, refreshToken);

                    return Ok(new JwtTokenResponse()
                    {
                        JwtToken = jwtTokenString,
                        RefreshToken = refreshToken.ToString("D"),
                    });


                }
                else
                {
                    Log.Warning("Failed login attempt {0}: invalid email or password", loginRequest.Email);

                    return BadRequest("Invalid email or password");
                }
            }
            else
            {
                Log.Warning("Failed login attempt {0}: not valid data", loginRequest.Email);
                return BadRequest("Bad Validation");
            }
        }

        /// <summary>
        /// Registration
        /// </summary>
        /// <param name="registrationRequest">
        /// Registration model.
        /// Password format: @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{5,16}$".
        /// Name format: @"^[a-zA-Z]+$".
        /// </param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/registration
        ///     {
        ///        "name": "Example",
        ///        "email": "exmple@email.ru",
        ///        "password": "Qwer123!!"
        ///        "confirmPassword": "Qwer123!!"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Successful registration. New user added.</response>
        /// <response code="400">Bad valid or user already exist.</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        [Route("registration")]
        public async Task<IActionResult> Registration([FromBody] RegistrationRequest registrationRequest)
        {
            ValidationResult result = await _serviceFactory
                .CreateRegistrationValidator()
                .ValidateAsync(registrationRequest);

            if (result.IsValid)
            {

                if (await _serviceFactory
                        .CreateIdentityService()
                        .RegistrationAsync(_serviceFactory
                            .CreateMapperService()
                            .Map<UserRegistrationDto>(registrationRequest)))
                {

                    return Ok();
                }
                else
                {
                    Log.Warning("Failed registration attempt {0}: user already exist", registrationRequest.Email);

                    return BadRequest("User already exist");
                }
            }
            else
            {
                Log.Warning("Failed login attempt {0}: not valid data", registrationRequest.Email);
                return BadRequest("Bad Validation");
            }
        }
        /// <summary>
        /// Refresh token
        /// </summary>
        /// <param name="refreshTokenRequest">
        /// </param>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /auth/refresh-token
        ///     {
        ///        "refreshToken": "213DED52-5C70-460A-BF1F-A4339F9499AD"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">Token valid. Return new Jwt and refresh token.</response>
        [HttpPost]
        [ProducesResponseType(typeof(JwtTokenResponse),StatusCodes.Status200OK)]
        [Route("refresh-token")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest refreshTokenRequest)
        {

            var tokens = await _serviceFactory
                .CreateJwtService()
                .RefreshTokenAsync(refreshTokenRequest.RefreshToken);

            return Ok(new JwtTokenResponse()
            {
                JwtToken = tokens.JwtToken,
                RefreshToken = tokens.RefreshToken,
            });
        }
    }
}
