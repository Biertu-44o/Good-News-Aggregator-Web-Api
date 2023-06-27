using FluentValidation;
using Web_Api_Controllers.RequestModels;

namespace Web_Api_Controllers.Validators
{
    public class SettingsValidator
    {
        public class LoginValidator : AbstractValidator<PutSettingsRequest>
        {
            public LoginValidator()
            {
                RuleFor(x => x.Name)
                    .NotNull()
                    .Matches(Patterns.NamePattern); ;
                
                RuleFor(x => x.PositiveRateFilter)
                    .GreaterThanOrEqualTo(0)
                    .LessThanOrEqualTo(10);

                RuleFor(x => x.Theme)
                    .NotNull()
                    .NotEmpty();
            }
        }
    }
}
