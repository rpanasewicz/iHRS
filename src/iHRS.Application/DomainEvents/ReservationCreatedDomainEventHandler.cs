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
        private readonly IRepository<Hotel> _hotelRepository;
        private readonly IMessageService _messageService;

        public ReservationCreatedDomainEventHandler(IMessageService messageService, IRepository<Hotel> hotelRepository)
        {
            _messageService = messageService;
            _hotelRepository = hotelRepository;
        }

        public async Task Handle(ReservationCreatedDomainEvent domainEvent)
        {
            var hotel = await _hotelRepository.GetAsync(domainEvent.Room.HotelId, h => h.MessageTemplates);

            var template = hotel.MessageTemplates.FirstOrDefault(t =>
                t.CommunicationMethod == CommunicationMethod.Email &&
                t.MessageType == MessageType.ReservationConfirmation);

            if (template is null)
            {
                return;
            }

            var message = SmartFormat.Smart.Format(template.Message, domainEvent.Reservation, domainEvent.Room.Hotel);

            await _messageService.SendMessage("email", message, domainEvent.Reservation.Customer.EmailAddress);
        }
    }
}
