using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;


public class CustomExceptionFilterAttribute : ExceptionFilterAttribute, IFilterMetadata
{
    public override void OnException(ExceptionContext context)
    {

        Log.Error(context.Exception, "An error occurred in the route {0}", context.Exception.Source);

        context.HttpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        context.Result = new ObjectResult("Internal Server Error");

        context.ExceptionHandled = true;


    }
}