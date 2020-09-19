using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System;
using System.Threading.Tasks;

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

        public async Task<Unit> Handle(DeleteHotelCommand cmd)
        {
            var hotel = await _hotelRepository.GetAsync(cmd.HotelId);

            if (hotel is null)
                throw new NotFoundException(nameof(Hotel), cmd.HotelId);

            await _hotelRepository.DeleteAsync(hotel);

            return Unit.Value;
        }
    }
}


