using FluentValidation;
using Web_Api_Controllers.RequestModels;

namespace Web_Api_Controllers.Validators
{
    public class RegistrationValidator:AbstractValidator<RegistrationRequest>
    {
        public RegistrationValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .Matches(Patterns.NamePattern);

            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .Matches(Patterns.PasswordPattern);

            RuleFor(x => x.ConfirmPassword)
                .Equal(x => x.Password);
        }
    }
}
