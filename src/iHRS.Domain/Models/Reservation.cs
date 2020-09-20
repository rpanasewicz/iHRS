using iHRS.Domain.Common;
using System;

namespace iHRS.Domain.Models
{
    public class Reservation : Entity
    {
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }

        public int NumberOfPersons { get; private set; }
        public Room Room { get; private set; }
        public Guid RoomId { get; private set; }

        public Customer Customer { get; private set; }
        public Guid CustomerId { get; private set; }

        public int StatusId { get; private set; }
        public ReservationStatus Status
        {
            get => Enumeration.FromValue<ReservationStatus>(StatusId);
            private set => StatusId = value.Id;
        }

        private Reservation() { }

        private Reservation(Guid reservationId, DateTime startDate, DateTime endDate, int numberOfPersons, ReservationStatus status, Customer customer, Room room)
        {
            Id = reservationId;
            StartDate = startDate;
            EndDate = endDate;
            NumberOfPersons = numberOfPersons;
            Room = room;
            Customer = customer;
            CustomerId = customer.Id;
            RoomId = room.Id;
            Status = status;
        }

        internal static Reservation CreateNew(DateTime startDate, DateTime endDate, int numberOfPersons, Customer customer, Room room)
        {
            return new Reservation(Guid.NewGuid(), startDate, endDate, numberOfPersons, ReservationStatus.New, customer, room);
        }
    }
}