using iHRS.Application.Common;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Hotels
{
    public class CreateHotelCommand : ICommand<Guid>
    {
        public string Name { get; }

        public CreateHotelCommand(string name)
        {
            Name = name;
        }
    }

    public class CreateHotelCommandHandler : ICommandHandler<CreateHotelCommand, Guid>
    {
        private readonly IRepository<Hotel> _hotelRepository;

        public CreateHotelCommandHandler(IRepository<Hotel> hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<Guid> Handle(CreateHotelCommand cmd)
        {
            var hotel = Hotel.CreateNew(cmd.Name);
            await _hotelRepository.AddAsync(hotel);
            return hotel.Id;
        }
    }
}
