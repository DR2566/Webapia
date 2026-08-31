using Microsoft.AspNetCore.Diagnostics;
using Webapia.Application.Common.Errors.DTOs;
using Webapia.Domain.Exceptions;

namespace Webapia.Api.ExceptionHandlers;

public sealed class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "An exception occurred: {Message}", exception.Message);

        var (statusCode, message) = exception switch
        {
            BadRequestException ex => (StatusCodes.Status400BadRequest, ex.Message),
            NotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "An internal server error occurred.")
        };

        httpContext.Response.StatusCode = statusCode;

        var response = new ErrorResponseDto(statusCode, message);

        await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
        return true;
    }
}