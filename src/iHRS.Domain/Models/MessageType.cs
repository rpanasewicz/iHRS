using iHRS.Domain.Common;

namespace iHRS.Domain.Models
{
    public class MessageType : Enumeration
    {
        public static readonly MessageType ReservationConfirmation = new MessageType(1, "ReservationConfirmation");

        public MessageType(int id, string name) : base(id, name)
        {
        }
    }
}