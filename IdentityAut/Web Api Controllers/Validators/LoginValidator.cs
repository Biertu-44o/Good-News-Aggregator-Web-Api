using FluentValidation;
using System.ComponentModel.DataAnnotations;
using Web_Api_Controllers.RequestModels;

namespace Web_Api_Controllers.Validators
{
    public class LoginValidator:AbstractValidator<LoginRequest>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).EmailAddress().Length(4, 20).NotNull();
            RuleFor(x => x.Password).Matches(Patterns.PasswordPattern).NotNull();
        }
    }
}