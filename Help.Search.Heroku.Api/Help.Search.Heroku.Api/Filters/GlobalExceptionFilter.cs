namespace Help.Search.Heroku.Api.Filters
{
    using Microsoft.AspNetCore.Mvc.Filters;
    using Microsoft.AspNetCore.Mvc;
    using System;
    using Help.Search.Heroku.Api.Middleware;
    using Microsoft.Extensions.Logging;

    public class GlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<GlobalExceptionFilter> _logger;

        public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            // Handle specific exception types or log them
            if (context.Exception is UnauthorizedAccessException)
            {
                context.Result = new UnauthorizedResult();
            }
            else
            {
                _logger.LogError(context.Exception, "Unhandled exception occurred.");

                context.Result = new ObjectResult(new { Message = "An error occurred while processing your request." })
                {
                    StatusCode = 500
                };

            }

            // Log the exception if needed
            _logger.LogError(context.Exception, "An error occurred");

            context.ExceptionHandled = true;
        }
    }

}
