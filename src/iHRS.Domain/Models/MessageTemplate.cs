using System;
using iHRS.Domain.Common;

namespace iHRS.Domain.Models
{
    public class MessageTemplate : Entity
    {
        public string Message { get; private set; }
        public Guid HotelId { get; private set; }
        public Hotel Hotel { get; private set; }

        public int MessageTypeId { get; private set; }
        public MessageType MessageType
        {
            get => Enumeration.FromValue<MessageType>(MessageTypeId);
            private set => MessageTypeId = value.Id;
        }

        public int CommunicationMethodId { get; private set; }
        public CommunicationMethod CommunicationMethod
        {
            get => Enumeration.FromValue<CommunicationMethod>(CommunicationMethodId);
            private set => CommunicationMethodId = value.Id;
        }

        private MessageTemplate()
        {
            
        }

        private MessageTemplate(Guid messageTemplateId, string message, Hotel hotel, MessageType messageType, CommunicationMethod communicationMethod)
        {
            Id = messageTemplateId;
            Message = message;
            HotelId = hotel.Id;
            Hotel = hotel;
            MessageType = messageType;
            CommunicationMethod = communicationMethod;
        }

        internal static MessageTemplate CreateNew(string message, Hotel hotel, MessageType type, CommunicationMethod communicationMethod)
        {
            return new MessageTemplate(Guid.NewGuid(), message, hotel, type, communicationMethod);
        }
    }
}
