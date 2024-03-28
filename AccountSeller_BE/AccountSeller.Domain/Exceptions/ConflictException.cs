using System.Runtime.Serialization;

namespace AccountSeller.Domain.Exceptions
{
    public class ConflictException : Exception, ISerializable
    {
        public string ConflictValue { get; set; }

        public ConflictException()
        {

        }

        public ConflictException(string message) : base(message)
        {

        }

        public ConflictException(string message, Exception innerException)
           : base(message, innerException)
        {
        }

        public ConflictException(string name, object key)
            : base($"{name}{key}")
        {
        }

        protected ConflictException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public ConflictException(string message, string conflictValue)
                : base(message)
        {
            this.ConflictValue = conflictValue;
        }

    }
}
