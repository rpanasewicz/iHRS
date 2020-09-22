using iHRS.Application.Queries.Dtos;
using System.Collections.Generic;

namespace iHRS.Application.Queries
{
    public interface IHotelQueries : IQuery
    {
        IEnumerable<HotelDto> GetAll();
    }
}
