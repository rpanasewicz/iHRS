using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents;
using iHRS.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iHRS.Domain.Models
{
    public class Room : Entity
    {
        public string RoomNumber { get; private set; }
        public Guid HotelId { get; private set; }
        public Hotel Hotel { get; private set; }

        public IEnumerable<Reservation> Reservations
        {
            get => _reservations.AsEnumerable();
            private set => _reservations = new HashSet<Reservation>(value);
        }

        private ISet<Reservation> _reservations;

        private Room() { } // For EF

        private Room(Guid roomId, string roomNumber, Hotel hotel, IEnumerable<Reservation> reservations = null)
        {
            if (hotel == null) throw new ArgumentNullException(nameof(hotel));
            if (string.IsNullOrEmpty(roomNumber))
                throw new ArgumentException("Value cannot be null or empty.", nameof(roomNumber));

            this.Id = roomId;
            RoomNumber = roomNumber;
            HotelId = hotel.Id;
            Hotel = hotel;
            Reservations = reservations ?? Enumerable.Empty<Reservation>();
        }

        internal static Room CreateNew(string roomNumber, Hotel hotel)
        {
            return new Room(Guid.NewGuid(), roomNumber, hotel);
        }

        public Reservation CreateReservation(DateTime fromDate, DateTime toDate, int numberOfPersons, Customer customer)
        {
            if (_reservations is null) throw new PropertyNotInitializedException(nameof(Reservations));

            if (_reservations.Any(r => r.StartDate < toDate && r.EndDate > fromDate))
                throw new RoomAlreadyReserved(); ;

            var reservation = Reservation.CreateNew(fromDate, toDate, numberOfPersons, customer, this);

            _reservations.Add(reservation);

            AddEvent(new ReservationCreatedDomainEvent(reservation, this));

            return reservation;
        }
    }
}
