using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace iHRS.Domain.Exceptions
{
    public class MessageTemplateAlreadyExist : DomainException
    {
        public MessageTemplateAlreadyExist() : base("Message template already exist.")
        {
        }

        public override string Code => "message_temple_exist";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;
    }
}
