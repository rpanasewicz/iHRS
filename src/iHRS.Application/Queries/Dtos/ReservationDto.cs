using System;

namespace iHRS.Application.Queries.Dtos
{
    public class ReservationDto
    {
        public Guid ReservationId { get; }
        public DateTime ReservationStartDate { get; }
        public DateTime ReservationEndDate { get; }

        public Guid RoomId { get; }
        public string RoomNumber { get; }

        public Guid HotelId { get; set; }
        public string HotelName { get; }

        public ReservationDto(Guid reservationId, DateTime reservationStartDate, DateTime reservationEndDate, Guid roomId, string roomNumber, Guid hotelId, string hotelName)
        {
            ReservationId = reservationId;
            ReservationStartDate = reservationStartDate;
            ReservationEndDate = reservationEndDate;
            RoomId = roomId;
            RoomNumber = roomNumber;
            HotelId = hotelId;
            HotelName = hotelName;
        }
    }
}
