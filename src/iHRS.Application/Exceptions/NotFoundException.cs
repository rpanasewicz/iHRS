using System;
using System.Net;

namespace iHRS.Application.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string resourceName, string resourceId) : base($"{resourceName} ({resourceId}) not found!")
        {
        }

        public NotFoundException(string resourceId) : base($"Resource ({resourceId}) not found!")
        {
        }

        public NotFoundException(string resourceName, Guid resourceId) : this(resourceName, resourceId.ToString())
        {

        }

        public NotFoundException(Guid resourceId) : this(resourceId.ToString())
        {

        }

        public override string Code => "not_found";
        public override HttpStatusCode StatusCode => HttpStatusCode.NotFound;
    }
}