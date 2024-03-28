
using AccountSeller.Application.Common.Services;
using AccountSeller.Domain.Exceptions;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AccountSeller.Presentation.Filters
{
    [AttributeUsage(
          AttributeTargets.Class
        | AttributeTargets.Enum
        | AttributeTargets.Interface
        | AttributeTargets.Delegate)]
    public sealed class ApiExceptionFilterAttribute : ExceptionFilterAttribute
    {
        private readonly TelemetryClient telemetryClient;

        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute(TelemetryClient telemetryClient)
        {
            // Register known exception types and handlers.
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                {typeof(ValidationException), HandleValidationException},
                {typeof(NotFoundException), HandleNotFoundException},
                {typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException},
                {typeof(ForbiddenAccessException), HandleForbiddenAccessException},
                {typeof(JsonReaderException), HandleJsonReaderException},
                {typeof(ArgumentException), HandleArgumentException},
                {typeof(ConflictException), HandleConflictException},
                {typeof(NotAcceptableException), HandleNotAcceptableAccessException},
                {typeof(DbUpdateConcurrencyException), HandleDbConcurrencyUpdate},
                {typeof(BusinessException), HandleBusinessException},
                {typeof(Exception), HandleUnhandledException},
            };

            this.telemetryClient = telemetryClient;
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            TrackingError(context);
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);

                var logModel = new LogModel(context.HttpContext);

                if (typeof(BusinessException) == context.Exception.GetType() || typeof(ValidationException) == context.Exception.GetType())
                {
                    Serilog.Log.Error(context.Exception.TraceToRootException(), context.Exception.GetType().Name + " {@LogModel}", logModel);
                }
                else
                {
                    Serilog.Log.Fatal(
                        context.Exception.TraceToRootException(),
                        "UN-HANDLED EXCEPTION: Type = " + context.Exception.GetType().Name + " {@LogModel}", logModel);
                }
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }

            HandleUnknownException(context);
        }

        private static void HandleValidationException(ExceptionContext context)
        {
            var exception = context.Exception as ValidationException;

            var details = new ValidationProblemDetails(exception?.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Detail = context.Exception?.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleJsonReaderException(ExceptionContext context)
        {
            var exception = context.Exception as JsonReaderException;

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Cannot parse JSON string to object.",
                Detail = exception!.Message
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = new BadRequestObjectResult(details);

            context.ExceptionHandled = true;
        }

        private static void HandleNotFoundException(ExceptionContext context)
        {
            var exception = context.Exception as NotFoundException;
            var detail = GetInnerException(context);

            var details = new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Title = "The specified resource was not found.",
                Detail = exception != null ? exception.Message : detail
            };

            context.Result = new NotFoundObjectResult(details);
            context.ExceptionHandled = true;
        }

        private static void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status401Unauthorized
            };

            context.ExceptionHandled = true;
        }

        private static void HandleForbiddenAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status403Forbidden,
                Title = "Forbidden",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Detail = context.Exception.Message
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status403Forbidden
            };

            context.ExceptionHandled = true;
        }

        private static void HandleUnknownException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred while processing your request.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = GetInnerException(context)
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };

            context.ExceptionHandled = true;
        }

        private void HandleArgumentException(ExceptionContext context)
        {
            string detail = string.Empty;
            var exception = context.Exception as ArgumentException;
            if (exception == null)
            {
                detail = GetInnerException(context);
            }

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Input is invalid",
                Detail = exception != null ? exception.Message : detail
            };

            context.Result = new BadRequestObjectResult(details);
            context.ExceptionHandled = true;
        }

        private void HandleConflictException(ExceptionContext context)
        {
            string detail = string.Empty;
            var exception = context.Exception as ConflictException;
            if (exception == null)
            {
                detail = GetInnerException(context);
            }

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "Resource conflict",
                Detail = exception != null ? exception.Message + " - " + exception.ConflictValue : detail
            };

            context.Result = new ConflictObjectResult(details);
            context.ExceptionHandled = true;
        }

        private void HandleBusinessException(ExceptionContext context)
        {
            string detail = string.Empty;
            var exception = context.Exception as BusinessException;
            if (exception == null)
            {
                detail = GetInnerException(context);
            }

            var details = new ProblemDetails()
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                Title = "An error occurred because of did not satisfied application business expectation.",
                Detail = exception != null ? exception?.Message : detail,
                Instance = exception?.ErrorCode,
            };

            var envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (envName != "Production" && exception?.InnerException != null)
            {
                details.Title = $"APP Environment: {envName}. \r\nError Message: {exception?.InnerException?.Message}. Detail:\r\n {exception?.InnerException?.StackTrace}";
            }

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status400BadRequest
            };
            context.ExceptionHandled = true;
        }

        private void HandleDbConcurrencyUpdate(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status412PreconditionFailed,
                Title = "Precondition failed",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Detail = GetInnerException(context)
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status412PreconditionFailed
            };

            try
            {
                Exception databaseException = context.Exception as DbUpdateConcurrencyException;
                databaseException = databaseException ?? context.Exception as DbUpdateConcurrencyException;
                telemetryClient?.TrackTrace($"[DbFailed] Cannot update data = {JsonConvert.SerializeObject(databaseException)}.");
            }
            catch
            {
                // Ignore if cannot get data from exception.
            }

            context.ExceptionHandled = true;
        }
        private void HandleNotAcceptableAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status406NotAcceptable,
                Title = "Not Acceptable",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.3",
                Detail = GetInnerException(context)
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status406NotAcceptable
            };

            context.ExceptionHandled = true;
        }

        private void HandleUnhandledException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = context.Exception?.Message,
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Detail = GetInnerException(context)
            };

            context.Result = new ObjectResult(details)
            {
                StatusCode = StatusCodes.Status406NotAcceptable
            };

            context.ExceptionHandled = true;
        }

        private static string GetInnerException(ExceptionContext context)
        {
            return context.Exception?.InnerException?.Message ?? context.Exception?.Message;
        }

        private void TrackingError(ExceptionContext context)
        {
            try
            {
                if (telemetryClient != null)
                {
                    telemetryClient.TrackException(context.Exception);
                    telemetryClient.TrackTrace(
                        $"{context.HttpContext.Request.Path}-{context.HttpContext.Request.Method}. Error = {GetInnerException(context)}.",
                        Microsoft.ApplicationInsights.DataContracts.SeverityLevel.Error);
                }
            }
            catch (Exception)
            {
                // Just ignore exception if cannot send data to Application Insights.
            }
        }
    }
}