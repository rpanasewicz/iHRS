using iHRS.Application.Queries.Dtos;
using System;
using System.Collections.Generic;

namespace iHRS.Application.Queries
{
    public interface ICustomerReservationQueries : IQuery
    {
        IEnumerable<ReservationDto> GetAll(Guid customerId);
    }
}
