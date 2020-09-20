using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iHRS.Application.Common;
using iHRS.Application.Exceptions;
using iHRS.Domain.Common;
using iHRS.Domain.Models;

namespace iHRS.Application.Commands.Hotels
{
    public class CreateMessageTemplateCommand : ICommand<Guid>
    {
        public string Message { get; }
        public Guid HotelId { get; }
        public int MessageType { get; }
        public int CommunicationMethod { get; }

        public CreateMessageTemplateCommand(string message, Guid hotelId, int messageType, int communicationMethod)
        {
            Message = message;
            HotelId = hotelId;
            MessageType = messageType;
            CommunicationMethod = communicationMethod;
        }
    }

    public class CreateMessageTemplateCommandHandler : ICommandHandler<CreateMessageTemplateCommand, Guid>
    {
        private readonly IRepository<Hotel> _hotelRepository;

        public CreateMessageTemplateCommandHandler(IRepository<Hotel> hotelRepository)
        {
            _hotelRepository = hotelRepository;
        }

        public async Task<Guid> Handle(CreateMessageTemplateCommand cmd)
        {
            var hotel = await _hotelRepository.GetAsync(cmd.HotelId, c => c.MessageTemplates);

            if (hotel is null) throw new NotFoundException(nameof(Hotel), cmd.HotelId);

            var template = hotel.AddMessageTemplate(cmd.Message, Enumeration.FromValue<MessageType>(cmd.MessageType),
                Enumeration.FromValue<CommunicationMethod>(cmd.CommunicationMethod));

            return template.Id;
        }
    }
}