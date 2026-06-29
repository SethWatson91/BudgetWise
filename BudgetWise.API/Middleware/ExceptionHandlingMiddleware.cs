namespace BudgetWise.API.Middleware
{
    using Microsoft.AspNetCore.Mvc;
    using System.Net;
    using System.Text.Json;

    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;
        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An unhandled exception occurred.");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var (statusCode, title) = exception switch
            {
                ArgumentException => (HttpStatusCode.BadRequest, "Bad Request"),
                InvalidOperationException => (HttpStatusCode.Conflict, "Conflict"),
                KeyNotFoundException => (HttpStatusCode.NotFound, "Not Found"),
                UnauthorizedAccessException => (HttpStatusCode.Unauthorized, "Unauthorized"),
                _ => (HttpStatusCode.InternalServerError, "Internal Server Error")
            };

            var problemDetails = new ProblemDetails
            {
                Type = $"https://tools.ietf.org/html/rfc9110#section-15.5.{(int)statusCode - 399}",
                Title = title,
                Status = (int)statusCode,
                Detail = exception.Message,
                Instance = context.Request.Path
            };

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            await context.Response.WriteAsync(JsonSerializer.Serialize(problemDetails));
        }
    }
}
