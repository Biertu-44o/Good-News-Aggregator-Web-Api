
namespace MVC.ValidationRules
{
    internal interface IValidatePatterns
    {
        protected const string NamePattern = @"^[a-zA-Z]+$";

        protected const string PasswordPattern =
            @"^(?=.*[a-z])(?=.*[A-Z])(?=.*[0-9])(?=.*[^a-zA-Z0-9])\S{5,16}$";

    }
}
