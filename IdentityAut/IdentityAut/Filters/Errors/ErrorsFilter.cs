using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace MVC.Filters.Errors;

public class CustomExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        Log.Error(context.Exception, "An error occurred in the controller {0}", 
            context.RouteData.Values["controller"]);
        
        context.ExceptionHandled= true;

        context.HttpContext.Response.StatusCode=500;
        Log.CloseAndFlush();
    }
}