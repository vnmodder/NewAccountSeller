using System.Runtime.Serialization;

namespace AccountSeller.Domain.Exceptions
{
    [Serializable]
    public class NotAcceptableException : Exception, ISerializable
    {
        public NotAcceptableException() { }

        public NotAcceptableException(string message) : base(message)
        {

        }


        public NotAcceptableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public NotAcceptableException(string name, object key)
            : base($"Role \"{name}\" ({key}) was not found.")
        {
        }

        protected NotAcceptableException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }
    }
}
