using AutoMapper;
using FluentValidation;
using FluentValidation.Results;
using IServices;
using IServices.Services;
using MVC.Models.AccountModels;
using MVC.Models.UserSettings;
using MVC.ValidationRules;

namespace MVC.Filters.Validation.ValidationRules
{
    internal static class AccountValidationHelper
    {

        internal static async Task<ValidationResult> AccountLoginValidator
            (UserLoginViewModel? model, IMapper mapper, IAuthService authService)
        {
            UserLoginViewValidate validator = new UserLoginViewValidate(mapper, authService);

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));

            if (result.IsValid)
            {
                result = await validator.ValidateAsync(model,
                    options =>
                        options.IncludeRuleSets("LoginCheck"));
            }

            return result;
        }


        internal static async Task<ValidationResult> AccountRegistrationValidator
            (UserRegistrationViewModel? model)
        {
            UserRegistrationViewValidate validator = new UserRegistrationViewValidate();

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));

            return result;
        }


        internal static async Task<ValidationResult> InfoSettingsValidator
            (NewUserSettingsViewModel? model)
        {
            UserSettingsViewValidate validator = new UserSettingsViewValidate();

            var result = await validator.ValidateAsync(model,
                options =>
                    options.IncludeRuleSets("PatternsCheck"));
            return result;
        }
    }
}
