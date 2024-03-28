namespace AccountSeller.Infrastructure.Common.Models
{
    public class RenderingResult
    {
        public bool Succeed { get; } = true;
        public string FailedAction { get; } = string.Empty;
        public Exception Exception { get; } = null;
        public Stream Result { get; } = null;

        public RenderingResult(bool isSucceed, string failedAction, Exception exception)
        {
            Succeed = isSucceed;
            FailedAction = failedAction;
            Exception = exception;
        }

        public RenderingResult(Stream fileStream)
        {
            Result = fileStream;
        }
    }
}
