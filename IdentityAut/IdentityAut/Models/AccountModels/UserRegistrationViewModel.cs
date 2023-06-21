using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using MVC.Filters.Validation.ValidationRules;
using MVC.ValidationRules;

namespace MVC.Models.AccountModels;

public class UserRegistrationViewModel: 
    IValidatePatterns
    ,ValidateErrors
{

    [Required(ErrorMessage = ValidateErrors.BadName)]
    [RegularExpression(IValidatePatterns.NamePattern
        , ErrorMessage =ValidateErrors.BadName)]
    public String Name { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoEmail)]
    [EmailAddress(ErrorMessage = ValidateErrors.BadEmail)]
    [Remote("CheckUserRegistrationExist", "Account",
        ErrorMessage = ValidateErrors.EmailExist)]
    public String Email { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoPassword)]
    [DataType(DataType.Password)]
    [RegularExpression(IValidatePatterns.PasswordPattern
        , ErrorMessage = ValidateErrors.BadPassword)]
    public String Password { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoConfirm)]
    [DataType(DataType.Password)]
    [Compare("Password"
        , ErrorMessage = ValidateErrors.BadConfirm)]
    public String ConfirmPassword { get; set; }

}