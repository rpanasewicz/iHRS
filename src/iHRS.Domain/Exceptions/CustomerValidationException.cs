using System.Net;

namespace iHRS.Domain.Exceptions
{
    public class CustomerValidationException : DomainException
    {
        public override string Code => "customer_validation_fail";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public CustomerValidationException() : base("Customer validation fail.")
        {
        }
    }
}
