using System;
using System.Threading;
using System.Threading.Tasks;
using iHRS.Application.Common;
using iHRS.Domain.Common;
using iHRS.Domain.Models;

namespace iHRS.Application.Commands.Hotels
{
    public class DeleteHotelCommand : ICommand
    {
        public DeleteHotelCommand(Guid hotelId)
        {
            HotelId = hotelId;
        }

        public Guid HotelId { get; }
    }

    public class DeleteHotelCommandHandler : ICommandHandler<DeleteHotelCommand>
    {
        private readonly IRepository<Hotel> _hotelRepository;

        public DeleteHotelCommandHandler(IRepository<Hotel> hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task Handle(DeleteHotelCommand cmd)
        {
            var hotel = await _hotelRepository.GetAsync(cmd.HotelId);

            await  _hotelRepository.DeleteAsync(hotel);
        }
    }
}


