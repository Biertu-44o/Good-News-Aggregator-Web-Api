using AutoMapper;
using IServices;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using MVC.Models.UserSettings;
using Serilog;
using MVC.Filters.Validation.ValidationRules;

namespace MVC.Filters.Validation
{

    public class SettingsValidationFilterAttribute : ActionFilterAttribute
    {
       

        public override async void OnActionExecuting(ActionExecutingContext context)
        {
            var formObject =
                context.ActionArguments.SingleOrDefault(p =>
                    p.Value is NewUserSettingsViewModel);

            var validationResult = await AccountValidationHelper
                .InfoSettingsValidator((NewUserSettingsViewModel)formObject.Value!);


            if (!validationResult.IsValid)
            {
                Log.Warning("Validation error occurred during settings change. IP: {0}");

                foreach (var Errors in validationResult.Errors)
                {
                    context.ModelState.AddModelError(Errors.PropertyName, Errors.ErrorMessage);
                }

                context.Result = new ViewResult
                {
                    ViewName = "Settings",
                };
            }



        }
    }

}
