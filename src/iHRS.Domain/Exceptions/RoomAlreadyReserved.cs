using System.Net;

namespace iHRS.Domain.Exceptions
{
    public class RoomAlreadyReserved : DomainException
    {
        public override string Code => "room_already_reserved";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public RoomAlreadyReserved() : base("Room already reserved.")
        {
        }
    }
}