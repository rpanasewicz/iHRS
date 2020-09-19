using iHRS.Domain.Common;
using System;

namespace iHRS.Domain.Models
{
    public class Reservation : Entity
    {
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }

        public int NumberOfPersons { get; }
        public Room Room { get; }
        public Guid RoomId { get; }

        public Customer Customer { get; }
        public Guid CustomerId { get; set; }

        private Reservation() { }

        private Reservation(DateTime startDate, DateTime endDate, int numberOfPersons, Customer customer, Room room)
        {
            StartDate = startDate;
            EndDate = endDate;
            NumberOfPersons = numberOfPersons;
            Room = room;
            Customer = customer;
            CustomerId = customer.Id;
            RoomId = room.Id;
        }

        internal static Reservation CreateInstance(DateTime startDate, DateTime endDate, int numberOfPersons, Customer customer, Room room)
        {
            return new Reservation(startDate, endDate, numberOfPersons, customer, room);
        }
    }
}