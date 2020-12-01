using System.Net;

namespace iHRS.Application.Exceptions
{
    public class PasswordNotChangedException : AppException
    {
        public override string Code => "password_not_changed";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public PasswordNotChangedException(string email) : base($"Password for user ({email}) has not been yet set up.")
        {
        }
    }
}
