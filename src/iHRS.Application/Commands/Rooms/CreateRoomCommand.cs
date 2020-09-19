using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System;
using System.Threading.Tasks;

namespace iHRS.Application.Commands.Rooms
{
    public class CreateRoomCommand : ICommand<Guid>
    {
        public Guid HotelId { get; }
        public string RoomNumber { get; }

        public CreateRoomCommand(Guid hotelId, string roomNumber)
        {
            HotelId = hotelId;
            RoomNumber = roomNumber;
        }
    }

    public class CreateRoomCommandHandler : ICommandHandler<CreateRoomCommand, Guid>
    {
        private readonly IRepository<Hotel> _hotelRepository;

        public CreateRoomCommandHandler(IRepository<Hotel> hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<Guid> Handle(CreateRoomCommand cmd)
        {
            var hotel = await _hotelRepository.GetAsync(cmd.HotelId, h => h.Rooms);

            if (hotel is null)
                throw new NotFoundException(nameof(Hotel), cmd.HotelId);

            var room = hotel.AddRoom(cmd.RoomNumber);

            return room.Id;
        }
    }
}