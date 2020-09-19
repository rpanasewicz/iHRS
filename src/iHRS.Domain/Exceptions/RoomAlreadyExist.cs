using System.Net;

namespace iHRS.Domain.Exceptions
{
    public class RoomAlreadyExist : DomainException
    {
        public override string Code => "room_already_exist";
        public override HttpStatusCode StatusCode => HttpStatusCode.BadRequest;

        public RoomAlreadyExist(string roomNumber) : base($"Room number ({roomNumber}) already exist in the hotel.")
        {
        }
    }
}
