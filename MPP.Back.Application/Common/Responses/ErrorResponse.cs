namespace MPP.Back.Application.Common.Responses
{
    public class ErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;

        public ErrorResponse(int statusCode, string message, object? details = null)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
