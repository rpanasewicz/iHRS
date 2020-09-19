using System.Net;

namespace iHRS.Domain.Exceptions
{
    public class PropertyNotInitializedException : DomainException
    {
        public override string Code => "property_not_initialized";
        public override HttpStatusCode StatusCode => HttpStatusCode.InternalServerError;

        public PropertyNotInitializedException(string propertyName) : base($"Property is not initialized. Consider calling Include. Property name: {propertyName}.")
        {
        }

    }
}