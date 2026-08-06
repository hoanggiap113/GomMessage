using GomMessage.Domain.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace GomMessage.Api.Middlewares
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;
        private readonly IHostEnvironment _env;
        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger,
            IHostEnvironment env) 
        {
            _logger = logger;
            _env = env;
        }
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
            if (exception is DomainException or ValidationException)
            {
                _logger.LogWarning("Business validation error: {Message}", exception.Message);
            }
            else
            {
                _logger.LogError(exception, "An unhandled system exception occurred: {Message}", exception.Message);
            }
            var (statusCode, title, detail) = exception switch
            {
                DomainException domainEx => (
                    GetDomainStatusCode(domainEx),
                    "Domain Validation Error",
                    domainEx.Message
                ),

                //ValidationException => (
                //                StatusCodes.Status400BadRequest,
                //                "Validation Error",
                //                "One or more validation failures have occurred." 
                //            ),
                FluentValidation.ValidationException validationEx => (
                    StatusCodes.Status400BadRequest,
                    "Validation Error",
                    validationEx.Message
                ),
                // Các lỗi Unhandled khác 
                _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                _env.IsDevelopment()
                    ? $"{exception.Message} ({exception.GetType().Name})" 
                    : "An unexpected error occurred. Please try again later." 
                )
            };

            var problemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Type = $"https://httpstatuses.com/{statusCode}"
            };

            if (_env.IsDevelopment() && statusCode == StatusCodes.Status500InternalServerError)
            {
                problemDetails.Extensions["exceptionType"] = exception.GetType().FullName;
                problemDetails.Extensions["stackTrace"] = exception.StackTrace?.Split(Environment.NewLine);
            }

            if (exception is DomainException domainException && domainException.ErrorCode is not null)
            {
                problemDetails.Extensions["errorCode"] = domainException.ErrorCode.Code;
            }

            if (exception is ValidationException validationException)
            {
                var errors = validationException.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                problemDetails.Extensions["errors"] = errors;
            }

            httpContext.Response.StatusCode = statusCode;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }

        private static int GetDomainStatusCode(DomainException domainException)
        {
            if (domainException.ErrorCode == null)
            {
                return StatusCodes.Status400BadRequest;
            }

            return domainException.ErrorCode.Code switch
            {
                "USER_ALREADY_EXISTS" => StatusCodes.Status409Conflict,
                "RESOURCE_NOT_FOUND" => StatusCodes.Status404NotFound,
                _ => StatusCodes.Status400BadRequest
            };
        }
    }
}
