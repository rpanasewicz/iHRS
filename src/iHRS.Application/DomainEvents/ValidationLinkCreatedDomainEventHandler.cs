using iHRS.Application.Services;
using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents;
using iHRS.Domain.DomainEvents.Abstractions;
using iHRS.Domain.Models;
using SmartFormat;
using System;
using System.Threading.Tasks;
using System.Web;

namespace iHRS.Application.DomainEvents
{
    public class ValidationLinkCreatedDomainEventHandler : IDomainEventHandler<ValidationLinkCreatedDomainEvent>
    {
        private readonly IRepository<MessageTemplate> _templateRepository;
        private readonly IMessageService _messageService;

        public ValidationLinkCreatedDomainEventHandler(IRepository<MessageTemplate> templateRepository, IMessageService messageService)
        {
            _templateRepository = templateRepository;
            _messageService = messageService;
        }

        public async Task HandleAsync(ValidationLinkCreatedDomainEvent @event)
        {
            var template = await _templateRepository.GetAsync(t =>
                t.HotelId == @event.Customer.HotelId && t.MessageTypeId == MessageType.CustomerLogin.Id);

            var formatArgs = new
            {
                Hotel = new
                {
                    @event.Customer.Hotel.Name
                },
                Customer = new
                {
                    @event.Customer.FirstName,
                    @event.Customer.LastName,
                },
                ValidationLink =
                    HttpUtility.UrlEncode(Convert.ToBase64String(@event.ValidationLink.Id.ToByteArray()))
            };

            var message = Smart.Format(template.Message, formatArgs);

            await _messageService.SendMessage(message, new[] { @event.Customer.EmailAddress }, "email");
        }
    }
}
