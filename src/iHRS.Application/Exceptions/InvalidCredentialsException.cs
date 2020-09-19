using System.Net;

namespace iHRS.Application.Exceptions
{
    public class InvalidCredentialsException : AppException
    {
        public override string Code { get; } = "invalid_credentials";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public InvalidCredentialsException() : base("Invalid credentials.")
        {

        }
    }
}