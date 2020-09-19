using iHRS.Domain.Common;
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
    }
}
