using AutoMapper;
using iHRS.Application.Queries.Common;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class HotelDto : IMapFrom<Hotel>
    {
        public string Name { get; private set; }
    }
}
