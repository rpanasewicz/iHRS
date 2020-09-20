using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Domain.Models;

namespace iHRS.Domain.DomainEvents
{
    public class ReservationCreatedDomainEvent : IDomainEvent
    {
        public Reservation Reservation { get;  }
        public Room Room { get;  }

        public ReservationCreatedDomainEvent(Reservation reservation, Room room)
        {
            Reservation = reservation;
            Room = room;
        }
    }
}
