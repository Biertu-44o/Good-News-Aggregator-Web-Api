using FluentValidation;
using MVC.Models.AccountModels;

namespace MVC.ValidationRules
{
    internal class UserRegistrationViewValidate :
        AbstractValidator<UserRegistrationViewModel>
        , IValidatePatterns, ValidateErrors
    {


        internal UserRegistrationViewValidate()
        {

            RuleSet("PatternsCheck", () =>
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoName)
                    .Matches(IValidatePatterns.NamePattern)
                    .WithMessage(ValidateErrors.BadName);

                RuleFor(x => x.Email)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoEmail)
                    .EmailAddress()
                    .WithMessage(ValidateErrors.BadEmail);

                RuleFor(x => x.Password)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoPassword)
                    .Matches(IValidatePatterns.PasswordPattern)
                    .WithMessage(ValidateErrors.BadPassword);

                RuleFor(x => x.ConfirmPassword)
                    .Equal(x => x.Password)
                    .WithMessage(ValidateErrors.BadConfirm);
            });

        }
    }
}
