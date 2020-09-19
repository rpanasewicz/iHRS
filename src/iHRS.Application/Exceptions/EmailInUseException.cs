using System.Net;

namespace iHRS.Application.Exceptions
{
    public class EmailInUseException : AppException
    {
        public override string Code { get; } = "email_in_use";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
        public string Email { get; }

        public EmailInUseException(string email) : base($"Email {email} is already in use.")
        {
            Email = email;
        }
    }
}