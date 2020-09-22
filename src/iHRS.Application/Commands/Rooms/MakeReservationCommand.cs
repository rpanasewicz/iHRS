using iHRS.Application.Auth;
using iHRS.Application.Common;
using iHRS.Domain.Common;
using iHRS.Domain.Models;
using System;
using System.Threading.Tasks;
using iHRS.Application.Exceptions;

namespace iHRS.Application.Commands.Rooms
{
    public class MakeReservationCommand : ICommand<Guid>
    {
        public Guid HotelId { get; }
        public Guid RoomId { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public int NumberOfPersons { get; }
        public string CustomerFirstName { get; }
        public string CustomerLastName { get; }
        public string CustomerEmailAddress { get; }
        public string CustomerPhoneNumber { get; }

        public MakeReservationCommand(Guid hotelId, Guid roomId, DateTime startDate, DateTime endDate, int numberOfPersons,
            string customerFirstName, string customerLastName, string customerEmailAddress, string customerPhoneNumber)
        {
            HotelId = hotelId;
            RoomId = roomId;
            StartDate = startDate;
            EndDate = endDate;
            NumberOfPersons = numberOfPersons;
            CustomerFirstName = customerFirstName;
            CustomerLastName = customerLastName;
            CustomerEmailAddress = customerEmailAddress;
            CustomerPhoneNumber = customerPhoneNumber;
        }
    }

    public class MakeReservationCommandHandler : ICommandHandler<MakeReservationCommand, Guid>
    {
        private readonly IRepository<Customer> _customerRepository;
        private readonly IRepository<Room> _roomRepository;
        private readonly IAuthProvider _auth;


        public MakeReservationCommandHandler(IRepository<Room> roomRepository, IRepository<Customer> customerRepository, IAuthProvider auth)
        {
            _roomRepository = roomRepository;
            _customerRepository = customerRepository;
            _auth = auth;
        }

        public async Task<Guid> Handle(MakeReservationCommand cmd)
        {
            var room = await _roomRepository.GetAsync(cmd.RoomId, r => r.Reservations, r => r.Hotel);

            if(room is null) throw new NotFoundException(nameof(Room), cmd.RoomId);

            var customer = _auth.CustomerId == default
                ? await _customerRepository.GetAsync(_auth.CustomerId)
                : Customer.CreateNew(cmd.CustomerFirstName, cmd.CustomerLastName, cmd.CustomerEmailAddress, cmd.CustomerPhoneNumber, room.Hotel);

            await _customerRepository.AddAsync(customer);

            var reservation = room.CreateReservation(cmd.StartDate, cmd.EndDate, cmd.NumberOfPersons, customer);

            return reservation.Id;
        }
    }
}