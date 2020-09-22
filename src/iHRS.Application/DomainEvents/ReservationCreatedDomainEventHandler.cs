using iHRS.Application.Services;
using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents;
using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Domain.Models;
using System.Linq;
using System.Threading.Tasks;

namespace iHRS.Application.DomainEvents
{
    public class ReservationCreatedDomainEventHandler : IDomainEventHandler<ReservationCreatedDomainEvent>
    {
        private readonly IRepository<Room> _roomRepository;
        private readonly IMessageService _messageService;

        public ReservationCreatedDomainEventHandler(IRepository<Room> roomRepository, IMessageService messageService)
        {
            _roomRepository = roomRepository;
            _messageService = messageService;
        }

        public async Task HandleAsync(ReservationCreatedDomainEvent @event)
        {
            if (@event.Room.Hotel is null)
                await _roomRepository.LoadProperty(@event.Room, e => e.Hotel);

            var template = @event.Room.Hotel.MessageTemplates.FirstOrDefault(t =>
                t.CommunicationMethod == CommunicationMethod.Email &&
                t.MessageType == MessageType.ReservationConfirmation);

            if (template is null)
            {
                return;
            }

            var message = SmartFormat.Smart.Format(template.Message, @event.Reservation, @event.Room.Hotel);

            await _messageService.SendMessage(message, new[] { @event.Reservation.Customer.EmailAddress }, "email");
        }
    }
}
