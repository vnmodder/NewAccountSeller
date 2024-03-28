using System.Runtime.Serialization;

namespace AccountSeller.Domain.Exceptions
{
    [Serializable]
    public class ForbiddenAccessException : Exception, ISerializable
    {
        public ForbiddenAccessException() { }

        public ForbiddenAccessException(string message) : base(message)
        {

        }


        public ForbiddenAccessException(string message, Exception innerException)
           : base(message, innerException)
        {
        }

        public ForbiddenAccessException(string roleName, object key)
            : base($"Role \"{roleName}\" ({key}) was not found.")
        {
        }

        protected ForbiddenAccessException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
