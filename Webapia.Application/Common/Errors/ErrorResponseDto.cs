namespace Webapia.Application.Common.Errors;

public record ErrorResponseDto
{
    public ErrorResponseDto(int errorCode, string errorMessage)
    {
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
        TimeStamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss");
    }

    public int ErrorCode { get; set; }
    public string ErrorMessage { get; set; }
    public string TimeStamp { get; set; }
}