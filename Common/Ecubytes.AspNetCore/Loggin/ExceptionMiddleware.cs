using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Ecubytes.Extensions.Logging
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILoggerFactory _loggerFactory;
        public ExceptionMiddleware(RequestDelegate next, ILoggerFactory loggerFactory)
        {            
            _loggerFactory = loggerFactory;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                _loggerFactory.CreateLogger(ex.TargetSite.DeclaringType.FullName).
                    LogError(ex,ex.Message);
                
                throw;
                //await HandleExceptionAsync(httpContext, ex);
            }
        }
        // private Task HandleExceptionAsync(HttpContext context, Exception exception)
        // {
        //     context.Response.ContentType = "application/json";
        //     context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //     return context.Response.WriteAsync(new ErrorDetails()
        //     {
        //         StatusCode = context.Response.StatusCode,
        //         Message = "Internal Server Error from the custom middleware."
        //     }.ToString());
        // }
    }
}
