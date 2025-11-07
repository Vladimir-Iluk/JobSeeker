using Bll.Exceptions;
using System.Net;
using System.Text.Json;

namespace WebApplication4.Middleware
{
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
                _logger.LogError(ex, "An unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var statusCode = exception switch
            {
                NotFoundException => HttpStatusCode.NotFound,
                ValidationException => HttpStatusCode.BadRequest,
                BusinessConflictException => HttpStatusCode.Conflict,
                FluentValidation.ValidationException => HttpStatusCode.UnprocessableEntity,
                _ => HttpStatusCode.InternalServerError
            };

            var problemDetails = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Title = exception switch
                {
                    NotFoundException => "Resource Not Found",
                    ValidationException => "Validation Error",
                    BusinessConflictException => "Business Conflict",
                    FluentValidation.ValidationException => "Validation Error",
                    _ => "An error occurred while processing your request"
                },
                Detail = exception.Message,
                Status = (int)statusCode,
                Instance = context.Request.Path
            };

            // Додаємо додаткові деталі для ValidationException
            if (exception is ValidationException validationEx)
            {
                problemDetails.Extensions["errors"] = validationEx.Errors;
            }

            // Додаємо додаткові деталі для FluentValidation.ValidationException
            if (exception is FluentValidation.ValidationException fluentValidationEx)
            {
                var errors = fluentValidationEx.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                problemDetails.Extensions["errors"] = errors;
            }

            context.Response.ContentType = "application/problem+json";
            context.Response.StatusCode = (int)statusCode;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var json = JsonSerializer.Serialize(problemDetails, options);
            return context.Response.WriteAsync(json);
        }
    }
}

