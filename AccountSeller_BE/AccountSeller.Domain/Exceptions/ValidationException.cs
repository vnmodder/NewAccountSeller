using System.Runtime.Serialization;
using ValidationFailure = FluentValidation.Results.ValidationFailure;

namespace AccountSeller.Domain.Exceptions
{
    [Serializable]
    public class ValidationException : Exception, ISerializable
    {
        public ValidationException()
            : base("One or more validation failures have occurred. Detail: ")
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(IEnumerable<ValidationFailure> failures)
            : this()
        {
            Errors = failures
                .GroupBy(e => e.PropertyName, e => e.ErrorMessage)
                .ToDictionary(failureGroup => failureGroup.Key, failureGroup => failureGroup.ToArray());
        }

        public ValidationException(Dictionary<string, string[]> errors)
            : this()
        {
            Errors = errors;
        }


        public ValidationException(string message) : base(message)
        {
            Errors = new Dictionary<string, string[]>();
        }

        public ValidationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ValidationException(SerializationInfo serializationInfo, StreamingContext streamingContext) : base(serializationInfo, streamingContext)
        {
        }

        public IDictionary<string, string[]> Errors { get; }
    }
}
