using Core.DTOs.Account;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web_Api_Controllers.ControllerFactory;
using Web_Api_Controllers.Filters.Errors;
using Web_Api_Controllers.RequestModels;
using Web_Api_Controllers.ResponseModels;

namespace Web_Api_Controllers.Controllers
{
    [ApiController] 
    [Authorize]
    [CustomExceptionFilter]
    [Route("settings")]
    public class SettingsController : ControllerBase
    {
        private readonly IServiceFactory _serviceFactory;

        public SettingsController
            (IServiceFactory serviceFactory)
        {
            _serviceFactory = serviceFactory ?? throw new NullReferenceException(nameof(serviceFactory));
        }

        /// <summary>
        /// Update user settings. Only authorized users.
        /// </summary>
        /// <param name="request">
        /// New user settings model.
        /// Theme: only "dark" or "default".
        /// Name format: @"^[a-zA-Z]+$".
        /// Positive filter rate: Greater than or equal to 0 and less than or equal to 10.
        /// </param>
        /// <remarks>
        /// Sample request:
        ///
        ///     PATCH /settings
        ///     {
        ///        "name": "Example",
        ///        "theme": "default",
        ///        "positiveRateFilter": "2"
        ///     }
        ///
        /// </remarks>
        /// <response code="200">User settings updated</response>
        /// <response code="400">Not valid model</response>
        /// <response code="401">User Unauthorized</response>
        /// <response code="404">User not found</response>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpPatch]
        public async Task<IActionResult> SetNewUserSettings([FromBody] PutSettingsRequest request)
        {
            ValidationResult result = await _serviceFactory
                .CreateSettingsValidator()
                .ValidateAsync(request);

            if (result.IsValid)
            {
                Boolean response = await _serviceFactory.CreateUserConfigService()
                    .UpdateUserSettingsAsync(
                        _serviceFactory.CreateMapperService().Map<userSettingsDTO>(request)
                        , HttpContext.User.Identity.Name);

                if (response)
                {
                    return Ok();
                }
                else
                {
                    return NotFound();
                }
            }

            return BadRequest();

        }

        /// <summary>
        /// Get user settings.Only authorized users.
        /// </summary>
        /// <remarks>
        /// Sample request:
        ///
        ///     GET /settings
        ///
        /// </remarks>
        /// <response code="200">Return user settings model</response>
        /// <response code="400">Not valid model</response>
        /// <response code="401">User Unauthorized</response>
        /// <response code="404">User not found</response>
        [ProducesResponseType(typeof(userSettingsDTO),StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet]
        public async Task<IActionResult> GetUserSettings()
        {
            if (HttpContext.User.Identity != null)
            {
                userSettingsDTO? infoSettings =
                    await _serviceFactory.CreateUserConfigService()
                        .GetUserInformationAsync(HttpContext.User.Identity.Name 
                                                 ?? throw new NullReferenceException("HttpContext.User.Identity.Name = null"));

                if (infoSettings != null)
                {
                    return Ok(infoSettings);
                }
            }

            return NotFound();
        }
    }
}
