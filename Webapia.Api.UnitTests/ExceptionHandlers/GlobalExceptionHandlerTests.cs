using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Webapia.Api.ExceptionHandlers;
using Webapia.Application.Common.Errors.DTOs;
using Webapia.Domain.Exceptions;
using Webapia.TestCommon;

namespace Webapia.Api.UnitTests.ExceptionHandlers;

public class GlobalExceptionHandlerTests
{
    private readonly TestLogger<GlobalExceptionHandler> _logger = new();
    private readonly GlobalExceptionHandler _sut;

    public GlobalExceptionHandlerTests()
    {
        _sut = new GlobalExceptionHandler(_logger);
    }

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
    public async Task TryHandleAsync_WhenBadRequestExceptionThrown_Returns400WithMessage()
    {
        var context = CreateHttpContext();
        var exception = new BadRequestException("Invalid pagination parameters.");

        var handled = await _sut.TryHandleAsync(context, exception, CancellationToken.None);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        context.Response.ContentType.Should().StartWith("application/json");

        var body = await ReadResponseBodyAsync(context);
        body.Should().NotBeNull();
        body!.StatusCode.Should().Be(StatusCodes.Status400BadRequest);
        body.Message.Should().Be("Invalid pagination parameters.");
    }

    [Fact]
    public async Task TryHandleAsync_WhenNotFoundExceptionThrown_Returns404WithMessage()
    {
        var context = CreateHttpContext();
        var exception = new NotFoundException("Product not found.");

        var handled = await _sut.TryHandleAsync(context, exception, CancellationToken.None);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status404NotFound);

        var body = await ReadResponseBodyAsync(context);
        body.Should().NotBeNull();
        body!.StatusCode.Should().Be(StatusCodes.Status404NotFound);
        body.Message.Should().Be("Product not found.");
    }

    [Fact]
    public async Task TryHandleAsync_WhenUnhandledExceptionThrown_Returns500WithGenericMessage()
    {
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("db connection lost");

        var handled = await _sut.TryHandleAsync(context, exception, CancellationToken.None);

        handled.Should().BeTrue();
        context.Response.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);

        var body = await ReadResponseBodyAsync(context);
        body.Should().NotBeNull();
        body!.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        body.Message.Should().Be("An internal server error occurred.");
        body.Message.Should().NotContain("db connection lost");
    }

    [Fact]
    public async Task TryHandleAsync_WhenExceptionThrown_LogsError()
    {
        var context = CreateHttpContext();
        var exception = new InvalidOperationException("very bad here");

        await _sut.TryHandleAsync(context, exception, CancellationToken.None);

        _logger.Logs.Should().ContainSingle(log =>
            log.Level == LogLevel.Error && log.Exception == exception);
    }
}