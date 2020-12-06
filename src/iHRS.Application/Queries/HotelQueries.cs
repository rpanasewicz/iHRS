using AutoMapper;
using iHRS.Application.Queries.Common;
using iHRS.Application.Queries.Dtos;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System.Collections.Generic;

namespace iHRS.Application.Queries
{
    public class HotelQueries : IQuery
    {
        private IRepository<Hotel> _hotelRepository;
        private IMapper _mapper;

        public HotelQueries(IRepository<Hotel> hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public IEnumerable<HotelDto> GetAll() => _hotelRepository.ProjectToListAsync<HotelDto>(_mapper.ConfigurationProvider).GetAwaiter().GetResult();

    }
}
