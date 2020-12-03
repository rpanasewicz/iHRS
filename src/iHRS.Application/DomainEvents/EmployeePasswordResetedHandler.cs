using iHRS.Application.Services;
using iHRS.Domain.DomainEvents;
using iHRS.Domain.DomainEvents.Abstractions;
using Microsoft.Extensions.Configuration;
using SmartFormat;
using System.Threading.Tasks;
using System.Web;

namespace iHRS.Application.DomainEvents
{
    public class EmployeePasswordResetedHandler : IDomainEventHandler<EmployeePasswordReseted>
    {
        private readonly IMessageService _messageService;
        private readonly IConfiguration _configuration;

        public EmployeePasswordResetedHandler(IMessageService messageService, IConfiguration configuration)
        {
            _messageService = messageService;
            _configuration = configuration;
        }

        public async Task Handle(EmployeePasswordReseted domainEvent)
        {
            var messageTemplate = "Hello { Employee.FirstName }. Your password has been reseted. Please use this link to login and set up your new password! { Link }";
            var linkTemplate = _configuration.GetValue<string>("LinkTemplates:EmployeeFirstSingin");

            var model = new { domainEvent.Employee, Link = string.Format(linkTemplate, HttpUtility.UrlEncode(domainEvent.Employee.Password)) };

            var message = Smart.Format(messageTemplate, model);

            await _messageService.SendMessage("email", message, domainEvent.Employee.EmailAddress);
        }
    }
}
