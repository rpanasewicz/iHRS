using System.Threading;
using System.Threading.Tasks;
using iHRS.Application.Common;
using iHRS.Domain.Common;
using iHRS.Domain.Models;

namespace iHRS.Application.Commands.Hotels
{
    public class CreateHotelCommand : ICommand
    {
        public string Name { get; set; }
    }

    public class CreateHotelCommandHandler : ICommandHandler<CreateHotelCommand>
    {
        private readonly IRepository<Hotel> _hotelRepository;

        public CreateHotelCommandHandler(IRepository<Hotel> hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task Handle(CreateHotelCommand cmd)
        {
            var hotel = Hotel.CreateNew(cmd.Name);
            await _hotelRepository.AddAsync(hotel);
;        }
    }
}
