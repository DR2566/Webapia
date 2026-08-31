using FluentAssertions;
using Webapia.Domain.Exceptions;

namespace Webapia.Domain.UnitTests.Exceptions;

public class BadRequestExceptionTests
{
    [Fact]
    public void DefaultConstructor_CreatesException_WithoutMessage()
    {
        var ex = new BadRequestException();

        ex.Should().BeAssignableTo<Exception>();
        ex.InnerException.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        var ex = new BadRequestException("Bad data formatting");

        ex.Message.Should().Be("Bad data formatting");
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsBoth()
    {
        var inner = new InvalidOperationException("db timeout");

        var ex = new BadRequestException("Bad data formatting", inner);

        ex.Message.Should().Be("Bad data formatting");
        ex.InnerException.Should().BeSameAs(inner);
    }
}