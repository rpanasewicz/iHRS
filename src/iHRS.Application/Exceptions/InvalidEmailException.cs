using System.Net;

namespace iHRS.Application.Exceptions
{
    public class InvalidEmailException : AppException
    {
        public override string Code { get; } = "invalid_email";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public InvalidEmailException(string email) : base($"Invalid email: {email}.")
        {
        }
    }
}