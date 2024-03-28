using AccountSeller.Domain.Messages;

namespace AccountSeller.Domain.Exceptions
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// Go to the root stack of an exception.
        /// </summary>
        /// <param name="exception">Exception which be traced.</param>
        /// <returns>Root exception in a new <see cref="Exception"/> instance.</returns>
        public static Exception TraceToRootException(this Exception exception)
        {
            Exception tempException = exception;

            while (tempException is AggregateException && tempException.InnerException != null)
            {
                tempException = tempException.InnerException;
            }

            return tempException;
        }

        /// <summary>
        /// This method just help us to reduce code.
        /// <br></br>
        /// Error code must be a <c>nameof(...Messages.ErrorCode)</c>.
        /// <br></br>
        /// Example: <c>(<see cref="ErrorMessages"/>.EM0001)</c>.
        /// </summary>
        /// <param name="errorCode"></param>
        /// <param name="exception"></param>
        /// <returns><see cref="BusinessException"/> instance to throw.</returns>
        public static BusinessException GenerateBusinessException(string errorCode, Exception exception = null)
        {
            string? message = errorCode[..2] switch
            {
                "EM" => ErrorMessages.ResourceManager.GetString(errorCode),
                "VM" => ValidationMessages.ResourceManager.GetString(errorCode),
                "WM" => WarningMessages.ResourceManager.GetString(errorCode),
                _ => InformationMessages.ResourceManager.GetString(errorCode),
            };

            return new BusinessException(errorCode: errorCode, message: message ?? string.Empty, innerException: exception);
        }
    }
}
