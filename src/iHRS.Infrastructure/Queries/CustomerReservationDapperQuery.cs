using iHRS.Application.Queries;
using iHRS.Application.Queries.Dtos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;

namespace iHRS.Infrastructure.Queries
{
    internal class CustomerReservationDapperQuery : DapperQueryBase, ICustomerReservationQueries
    {
        public CustomerReservationDapperQuery(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<ReservationDto> GetAll(Guid customerId)
        {
            return Get<ReservationDto>(@"
                        SELECT 
	                        reservation.ReservationId,
	                        reservation.StartDate as ReservationStartDate,
	                        reservation.EndDate as ReservationEndDate,
	                        room.RoomId,
	                        room.RoomNumber,
	                        hotel.HotelId,
	                        hotel.Name as HotelName
                        FROM Reservations as reservation
                        INNER JOIN Rooms as room on reservation.RoomId = room.RoomId
                        INNER JOIN Hotels as hotel on room.HotelId = hotel.HotelId
                        WHERE reservation.CustomerId = @customerId
                       ", new { customerId });
        }
    }
}
