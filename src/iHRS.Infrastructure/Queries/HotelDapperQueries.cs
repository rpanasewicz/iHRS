using iHRS.Application.Queries;
using iHRS.Application.Queries.Dtos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace iHRS.Infrastructure.Queries
{
    internal class HotelDapperQueries : DapperQueryBase, IHotelQueries
    {
        public HotelDapperQueries(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<HotelDto> GetAll() =>
            Get<HotelDto>(@"
                SELECT HotelId, Name
                FROM dbo.Hotels
                WHERE (ExpirationDate IS NULL OR ExpirationDate > GETUTCDATE())
                ");
    }
}
