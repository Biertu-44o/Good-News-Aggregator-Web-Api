using FluentValidation;
using MVC.Models.UserSettings;

namespace MVC.ValidationRules
{
    internal class UserSettingsViewValidate :
        AbstractValidator<NewUserSettingsViewModel>
        , IValidatePatterns
        , ValidateErrors
    {

        internal UserSettingsViewValidate()
        {

            RuleSet("PatternsCheck", () =>
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .WithMessage(ValidateErrors.NoName)
                    .Matches(IValidatePatterns.NamePattern)
                    .WithMessage(ValidateErrors.BadName);


                RuleFor(x => x.PositiveRateFilter).NotNull()
                    .WithMessage(ValidateErrors.NoRateFilter)
                    .GreaterThan(0)
                    .WithMessage(ValidateErrors.BadRateFilter)
                    .LessThan(10)
                    .WithMessage(ValidateErrors.BadRateFilter);
            });


        }
    }
}
