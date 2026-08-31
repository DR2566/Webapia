namespace Webapia.Application.Common.Errors.DTOs;

public record ErrorResponseDto
{
    public ErrorResponseDto(
        int statusCode,
        string message,
        IDictionary<string, string[]>? errors = null)
    {
        StatusCode = statusCode;
        Message = message;
        TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
        Errors = errors;
    }

    public int StatusCode { get; init; }
    public string Message { get; init; }
    public string TimeStamp { get; init; }
    public IDictionary<string, string[]>? Errors { get; init; }
}