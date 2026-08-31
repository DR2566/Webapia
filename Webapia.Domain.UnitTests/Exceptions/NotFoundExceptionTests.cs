using FluentAssertions;
using Webapia.Domain.Exceptions;

namespace Webapia.Domain.UnitTests.Exceptions;

public class NotFoundExceptionTests
{
    [Fact]
    public void DefaultConstructor_CreatesException_WithoutMessage()
    {
        var ex = new NotFoundException();

        ex.Should().BeAssignableTo<Exception>();
        ex.InnerException.Should().BeNull();
    }

    [Fact]
    public void Constructor_WithMessage_SetsMessage()
    {
        var ex = new NotFoundException("Product not found");

        ex.Message.Should().Be("Product not found");
    }

    [Fact]
    public void Constructor_WithMessageAndInnerException_SetsBoth()
    {
        var inner = new InvalidOperationException("db timeout");

        var ex = new NotFoundException("Product not found", inner);

        ex.Message.Should().Be("Product not found");
        ex.InnerException.Should().BeSameAs(inner);
    }
}