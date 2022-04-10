using Api.Service.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace Api.Service.CustomTags
{
    public class CustomExceptionFilterAttribute : ExceptionFilterAttribute
    {

        private ILogger<CustomExceptionFilterAttribute> _logger; // log4net

        public CustomExceptionFilterAttribute(ILogger<CustomExceptionFilterAttribute> logger)
        {
            _logger = logger;
        }

        public override void OnException(ExceptionContext context)
        {
            HttpStatusCode status = HttpStatusCode.InternalServerError;
            String message = String.Empty;

            var exceptionType = context.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
                status = HttpStatusCode.Unauthorized;
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
                status = HttpStatusCode.NotImplemented;
            }
            else if (exceptionType == typeof(AppException))
            {
                message = "Unknown Application Error ";
                status = HttpStatusCode.InternalServerError;
            }
            else
            {
                message = "An Error has occured. Reference code ";
                status = HttpStatusCode.NotFound;
            }
            context.ExceptionHandled = true;

            HttpResponse response = context.HttpContext.Response;
            response.StatusCode = (int)status;
            response.ContentType = "application/json";

            // Create a Guid and use it to reference the error in the log file
            // Log the error and return the guid to caller
            Guid guid = Guid.NewGuid();
            string logmessage = guid.ToString() + " - " + context.Exception.StackTrace + " End of Error";
            _logger.LogError(logmessage);

            context.Result = new JsonResult(message + " " + guid);

            base.OnException(context);
        }
    }
}
