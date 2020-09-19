using System;
using System.Net;

namespace iHRS.Domain.Exceptions
{
    public abstract class DomainException : Exception
    {
        public abstract string Code { get; }
        public abstract HttpStatusCode StatusCode { get; }

        protected DomainException(string message) : base(message)
        {

        }
    }
}
