namespace AccountSeller.Domain.Models

{
    public class ErrorResponse
    {
        public string? ErrorCode { get; set; }
        public string? Message { get; set; }

        public ErrorResponse(string errorCode, string message)
        {
            ErrorCode = errorCode;
            Message = message;
        }
    }
}
