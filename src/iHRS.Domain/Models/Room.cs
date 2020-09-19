using iHRS.Domain.Common;
using System;

namespace iHRS.Domain.Models
{
    public class Room : Entity
    {
        public string RoomNumber { get; private set; }
        public Guid HotelId { get; private set; }
        public Hotel Hotel { get; private set; }

        public Room() { } // For EF

        public Room(string roomNumber, Hotel hotel)
        {
            RoomNumber = roomNumber;
            HotelId = hotel.Id;
            Hotel = hotel;
        }
    }
}
