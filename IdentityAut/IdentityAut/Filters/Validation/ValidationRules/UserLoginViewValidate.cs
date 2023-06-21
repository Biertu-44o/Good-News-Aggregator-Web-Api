using AutoMapper;
using Core.DTOs.Account;
using FluentValidation;
using IServices.Services;
using MVC.Models.AccountModels;

namespace MVC.ValidationRules
{
    internal class UserLoginViewValidate : AbstractValidator<UserLoginViewModel>, IValidatePatterns, ValidateErrors
    {
        internal UserLoginViewValidate(IMapper mapper,IAuthService authService)
        {

            RuleSet("PatternsCheck", () =>
            {
                RuleFor(x => x.Email).NotNull()
                    .WithMessage(ValidateErrors.NoEmail)
                    .EmailAddress()
                    .WithMessage(ValidateErrors.BadEmail);

                RuleFor(x => x.Password)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoPassword)
                    .Matches(IValidatePatterns.PasswordPattern)
                    .WithMessage(ValidateErrors.BadPassword);
            });
            RuleSet("LoginCheck", () =>
            {
                RuleFor(x => x)
                .MustAsync(async (x, concellation) =>
                {

                    if (await authService.LoginAsync(mapper.Map<UserLoginDto>(x)))
                    {
                        return true;
                    }


                    return false;
                }).WithMessage(ValidateErrors.BadTry);
            });

        }

    }
}
