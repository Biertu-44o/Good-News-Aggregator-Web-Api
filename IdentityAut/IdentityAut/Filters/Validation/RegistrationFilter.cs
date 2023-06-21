using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Core.DTOs.Account;
using System.IO.Pipelines;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using MVC.Models.AccountModels;
using Serilog;
using MVC.Filters.Validation.ValidationRules;

namespace MVC.Filters.Validation
{
    public class RegistrationValidationFilterAttribute : ActionFilterAttribute
    {


        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var formObject =
                context.ActionArguments.SingleOrDefault(p =>
                    p.Value is UserRegistrationViewModel);

            var validationResult = await AccountValidationHelper
                .AccountRegistrationValidator(((UserRegistrationViewModel)formObject.Value!));


            if (!validationResult.IsValid)
            {
                var ip = context.HttpContext.Connection.RemoteIpAddress?.ToString();
                
                Log.Warning("Validation error occurred during user registration. IP: {0}", ip);
                
                foreach (var errors in validationResult.Errors)
                {
                    context.ModelState.AddModelError(errors.PropertyName, errors.ErrorMessage);
                }
                
                context.Result = new ViewResult
                {
                    ViewName = "Registration",
                };
            }



        }
    }
}
