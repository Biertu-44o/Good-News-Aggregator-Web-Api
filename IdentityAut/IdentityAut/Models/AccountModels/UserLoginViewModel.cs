using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using MVC.Filters.Validation.ValidationRules;
using MVC.ValidationRules;

namespace MVC.Models.AccountModels;

public class UserLoginViewModel:IValidatePatterns,ValidateErrors
{
    [Required(ErrorMessage =ValidateErrors.NoEmail )]
    [EmailAddress(ErrorMessage = ValidateErrors.BadEmail)]
    [Remote ("CheckUserLoginExist", "Account"
        , ErrorMessage = ValidateErrors.BadTry)]
    public String Email { get; set; }

    [Required(ErrorMessage = ValidateErrors.NoPassword)]
    [DataType(DataType.Password)]
    [RegularExpression(IValidatePatterns.PasswordPattern
        , ErrorMessage = ValidateErrors.BadPassword)]
    public String Password { get; set; }

    [AllowNull]
    public String? ReturnUrl { get; set; }=String.Empty;

}