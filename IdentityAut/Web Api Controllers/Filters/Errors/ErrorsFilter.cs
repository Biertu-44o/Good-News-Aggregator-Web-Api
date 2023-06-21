using Serilog;
using System.Net;
using System.Web.Http.Filters;

namespace Web_Api_Controllers.Filters.Errors;

public class CustomExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(HttpActionExecutedContext actionExecutedContext)
    {
        Log.Error(actionExecutedContext.Exception, "An error occurred in the route {0}", 
            actionExecutedContext.Exception.Source);

        String errorMessage = "Operation Error";
        
        var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
        {
            Content = new StringContent(errorMessage),
            ReasonPhrase = "Internal Server Error"
        };

        actionExecutedContext.Response = response;

        Log.CloseAndFlush();
    }
}