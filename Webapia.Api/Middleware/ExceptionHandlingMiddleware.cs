using Webapia.Application.Common.Errors.DTOs;
using Webapia.Domain.Exceptions;

namespace Webapia.Api.Middleware;

public class ExceptionHandlingMiddleware
{
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;
    private readonly RequestDelegate _next;

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
            _logger.LogError(ex, "Unhandled exception occurred");
            await HandleExceptionAsync(context, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message) = exception switch
        {
            BadRequestException ex => (StatusCodes.Status400BadRequest, ex.Message),
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "An internal server error occurred.")
        };

        context.Response.StatusCode = statusCode;

        var response = new ErrorResponseDto(statusCode, message);

        return context.Response.WriteAsJsonAsync(response);
    }
}