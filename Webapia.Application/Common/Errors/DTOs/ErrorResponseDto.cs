namespace Webapia.Application.Common.Errors.DTOs;

public record ErrorResponseDto
{
    public ErrorResponseDto(int statusCode, string message)
    {
        StatusCode = statusCode;
        Message = message;
        TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public int StatusCode { get; set; }
    public string Message { get; set; }
    public string TimeStamp { get; set; }
}