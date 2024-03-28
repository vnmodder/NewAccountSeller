using AccountSeller.Application.Common.Interfaces;
using AccountSeller.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using System.Dynamic;
using System.Security.Claims;

namespace AccountSeller.Application.Common.Services
{
    public class LogService : ILogService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LogService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public void Info(
            string message,
            object request = null,
            string stepTrace = null,
            Dictionary<string, object> traceObjects = null)
        {
            if (_httpContextAccessor != null)
            {
                var logModel = new LogModel(_httpContextAccessor.HttpContext, stepTrace: stepTrace, requestModel: request, traceObjects: traceObjects);
                Serilog.Log.Information(message, logModel);
            }
            else
            {
                Serilog.Log.Information(message, stepTrace);
            }
        }

        public void Warn(
            string message,
            object request = null,
            string stepTrace = null,
            Dictionary<string, object> traceObjects = null)
        {
            if (_httpContextAccessor != null)
            {
                var logModel = new LogModel(_httpContextAccessor.HttpContext, stepTrace: stepTrace, requestModel: request, traceObjects: traceObjects);
                Serilog.Log.Warning(message, logModel);
            }
            else
            {
                Serilog.Log.Warning(message, stepTrace);
            }
        }

        public void Error(
            string message,
            Exception exception,
            object request = null,
            string stepTrace = null,
            Dictionary<string, object> traceObjects = null)
        {
            if (_httpContextAccessor != null)
            {
                var logModel = new LogModel(_httpContextAccessor.HttpContext, stepTrace: stepTrace, requestModel: request, traceObjects: traceObjects);

                Serilog.Log.Error(
                    exception: exception.TraceToRootException(), message + " {@LogModel}", logModel);
            }
            else
            {
                Serilog.Log.Error(exception: exception.TraceToRootException(), message, stepTrace);
            }
        }
    }

    public class LogModel
    {
        public object? UserActionInformation { get; }
        public object? HttpHeader { get; }
        public object? RouteValues { get; }
        public object? QueryParameters { get; }
        public object? RequestModel { get; }
        public object? TraceObjectModels { get; }
        public string StepTrace { get; } = string.Empty;

        public LogModel(
            HttpContext httpContext,
            string stepTrace = null,
            object requestModel = null,
            Dictionary<string, object> traceObjects = null)
        {
            TimeZoneInfo clientTimeZone = GetTimeZone(httpContext?.Request);
            UserActionInformation = new
            {
                UserId = httpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
                UserRole = httpContext?.User?.FindFirstValue(ClaimTypes.Role),
                Timezone = clientTimeZone.DisplayName,
                ClientTime = $"{TimeZoneInfo.ConvertTime(DateTime.Now, TimeZoneInfo.Local, clientTimeZone)}",
            };

            HttpHeader = new
            {
                Path = httpContext?.Request?.Path,
                Method = httpContext?.Request?.Method,
                ContentType = httpContext?.Request?.ContentType,
                ClientAgent = httpContext?.Request?.Headers?.UserAgent,
            };

            RouteValues = httpContext?.Request?.RouteValues != null
                ? DictionaryToObject(httpContext?.Request?.RouteValues.ToDictionary(x => x.Key, x => x.Value))
                : null;

            Dictionary<string, object> queryParams = new Dictionary<string, object>();

            foreach (var param in httpContext?.Request?.Query)
            {
                queryParams.Add(param.Key, param.Value);
            }

            QueryParameters = DictionaryToObject(queryParams);

            RequestModel = requestModel;

            if (traceObjects != null)
            {
                TraceObjectModels = DictionaryToObject(traceObjects);
            }

            StepTrace = stepTrace;
        }

        private static dynamic DictionaryToObject(IDictionary<string, object> dictionary)
        {
            var expandoObj = new ExpandoObject();
            var expandoObjCollection = (ICollection<KeyValuePair<string, object>>)expandoObj;

            foreach (var keyValuePair in dictionary)
            {
                expandoObjCollection.Add(keyValuePair);
            }
            dynamic eoDynamic = expandoObj;
            return eoDynamic;
        }

        private static TimeZoneInfo GetTimeZone(HttpRequest request)
        {
            var defaultTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");
            if (request == null)
            {
                return defaultTimeZone;
            }

            try
            {
                return TimeZoneInfo.FindSystemTimeZoneById(request?.Headers["Timezone"]);

            }
            catch
            {
                return defaultTimeZone;
            }
        }
    }
}
