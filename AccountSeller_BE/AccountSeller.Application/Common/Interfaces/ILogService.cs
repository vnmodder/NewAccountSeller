namespace AccountSeller.Application.Common.Interfaces
{
    public interface ILogService
    {
        void Error(string message, Exception exception, object request = null, string stepTrace = null, Dictionary<string, object> traceObjects = null);
        void Info(string message, object request = null, string stepTrace = null, Dictionary<string, object> traceObjects = null);
        void Warn(string message, object request = null, string stepTrace = null, Dictionary<string, object> traceObjects = null);
    }
}
