using System;
using System.Net;

namespace iHRS.Application.Exceptions
{
    public abstract class AppException : Exception
    {
        public abstract string Code { get; }
        public abstract HttpStatusCode StatusCode { get; }

        protected AppException(string message) : base(message)
        {

        }
    }
}
