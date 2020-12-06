using iHRS.Application.Queries.Common;
using iHRS.Domain.Models;
using System;

namespace iHRS.Application.Queries.Dtos
{
    public class HotelDto : IMapFrom<Hotel>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
