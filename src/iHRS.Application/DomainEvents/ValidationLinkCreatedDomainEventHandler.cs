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
        private readonly IRepository<Customer> _customerRepository;
        private readonly IMessageService _messageService;

        public ValidationLinkCreatedDomainEventHandler(IRepository<MessageTemplate> templateRepository,
            IMessageService messageService, IRepository<Customer> customerRepository)
        {
            _templateRepository = templateRepository;
            _messageService = messageService;
            _customerRepository = customerRepository;
        }

        public async Task Handle(ValidationLinkCreatedDomainEvent domainEvent)
        {
            var template = await _templateRepository.GetAsync(t =>
                t.HotelId == domainEvent.Customer.HotelId && t.MessageTypeId == MessageType.CustomerLogin.Id);

            var formatArgs = new
            {
                Hotel = new
                {
                    domainEvent.Customer.Hotel.Name
                },
                Customer = new
                {
                    domainEvent.Customer.FirstName,
                    domainEvent.Customer.LastName,
                },
                ValidationLink =
                    HttpUtility.UrlEncode(Convert.ToBase64String(domainEvent.ValidationLink.Id.ToByteArray()))
            };

            var message = Smart.Format(template.Message, formatArgs);

            await _messageService.SendMessage("email", message, domainEvent.Customer.EmailAddress);
        }
    }
}
