using System.Runtime.Serialization;

namespace AccountSeller.Domain.Exceptions
{
    public class BusinessException : Exception, ISerializable
    {
        public string ErrorCode { get; set; }
        public BusinessException()
        {

        }

        public BusinessException(string message) : base(message)
        {

        }

        public BusinessException(string message, Exception innerException)
           : base(message, innerException)
        {
        }

        public BusinessException(string name, object key)
            : base($"A business exception have been occurred: {name}{key}.")
        {
        }

        protected BusinessException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public BusinessException(string message, string errorCode)
                : base(message)
        {
            this.ErrorCode = errorCode;
        }

        public BusinessException(string message, string errorCode, Exception innerException)
                : base(message, innerException)
        {
            this.ErrorCode = errorCode;
        }
    }
}
