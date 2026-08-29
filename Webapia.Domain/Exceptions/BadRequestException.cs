namespace Webapia.Domain.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException() : base()
    {
    }
    public BadRequestException(string message) : base(message)
    {
    }
    public BadRequestException(string message, Exception e) : base(message, e)
    {
    }
}