using Dapper;
using iHRS.Application.Queries;
using iHRS.Application.Queries.Dtos;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;

namespace iHRS.Infrastructure.Queries
{
    internal class HotelDapperQueries : DapperQueryBase, IHotelQueries
    {
        public IEnumerable<HotelDto> GetAll() =>
            Get(db => db.Query<HotelDto>(
                @"
                SELECT HotelId, Name
                FROM dbo.Hotels
                WHERE (ExpirationDate IS NULL OR ExpirationDate > GETUTCDATE())
                "));

        public HotelDapperQueries(IConfiguration configuration) : base(configuration)
        {
        }
    }


}
