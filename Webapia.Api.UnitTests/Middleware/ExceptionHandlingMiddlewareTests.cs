using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Webapia.Api.Middleware;
using Webapia.TestCommon;
using Webapia.Application.Common.Errors.DTOs;
using Webapia.Domain.Exceptions;

namespace Webapia.Api.UnitTests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private readonly TestLogger<ExceptionHandlingMiddleware> _logger = new();

    private static DefaultHttpContext CreateHttpContext()
    {
        var context = new DefaultHttpContext();
        context.Response.Body = new MemoryStream();
        return context;
    }

    private static async Task<ErrorResponseDto?> ReadResponseBodyAsync(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        return await JsonSerializer.DeserializeAsync<ErrorResponseDto>(
            context.Response.Body,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    [Fact]
    public async Task InvokeAsync_WhenNoExceptionThrown_CallsNextAndDoesNotModifyResponse()
    {
        var context = CreateHttpContext();
        var nextCalled = false;
        RequestDelegate next = _ =>
        {
            nextCalled = true;
            return Task.CompletedTask;
        };
        var sut = new ExceptionHandlingMiddleware(next, _logger);

        await sut.InvokeAsync(context);

        nextCalled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status200OK); // default
    }

    [Fact]
    public async Task InvokeAsync_WhenBadRequestExceptionThrown_Returns400WithMessage()
    {
        var context = CreateHttpContext();
        RequestDelegate next = _ => throw new BadRequestException("Invalid pagination parameters.");
        var sut = new ExceptionHandlingMiddleware(next, _logger);

        await sut.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        context.Response.ContentType.Should().StartWith("application/json");

        ErrorResponseDto? body = await ReadResponseBodyAsync(context);
        body!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        body.Message.Should().Be("Invalid pagination parameters.");
    }

    [Fact]
    public async Task InvokeAsync_WhenNotFoundExceptionThrown_Returns404WithMessage()
    {
        var context = CreateHttpContext();
        RequestDelegate next = _ => throw new NotFoundException("Product not found.");
        var sut = new ExceptionHandlingMiddleware(next, _logger);

        await sut.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var body = await ReadResponseBodyAsync(context);
        body!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        body.Message.Should().Be("Product not found.");
    }

    [Fact]
    public async Task InvokeAsync_WhenUnhandledExceptionThrown_Returns500WithGenericMessage()
    {
        var context = CreateHttpContext();
        RequestDelegate next = _ => throw new InvalidOperationException("db connection lost");
        var sut = new ExceptionHandlingMiddleware(next, _logger);

        await sut.InvokeAsync(context);

        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var body = await ReadResponseBodyAsync(context);
        body!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        body.Message.Should().Be("An internal server error occurred.");

        // Ensures the raw exception message ("db connection lost") never leaks to the client!
        body.Message.Should().NotContain("db connection lost");
    }

    [Fact]
    public async Task InvokeAsync_WhenExceptionThrown_LogsError()
    {
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("very bad here");
        RequestDelegate next = _ => throw exception;
        var sut = new ExceptionHandlingMiddleware(next, _logger);

        await sut.InvokeAsync(context);

        _logger.Logs.Should().ContainSingle(log =>
            log.Level == LogLevel.Error && log.Exception == exception);
    }
}