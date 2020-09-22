using iHRS.Domain.Common;
using iHRS.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace iHRS.Domain.Models
{
    public class Hotel : Entity
    {
        public string Name { get; private set; }

        public IEnumerable<Room> Rooms
        {
            get => _rooms.AsEnumerable();
            private set => _rooms = new HashSet<Room>(value);
        }
        public IEnumerable<MessageTemplate> MessageTemplates
        {
            get => _messageTemplates.AsEnumerable();
            private set => _messageTemplates = new HashSet<MessageTemplate>(value);
        }

        private ISet<Room> _rooms;
        private ISet<MessageTemplate> _messageTemplates;

        private Hotel() { } // For EF

        private Hotel(Guid id, string name, IEnumerable<Room> rooms = null, IEnumerable<MessageTemplate> messageTemplates = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Id = id;
            Name = name;
            Rooms = rooms ?? Enumerable.Empty<Room>();
            MessageTemplates = messageTemplates ?? Enumerable.Empty<MessageTemplate>();
        }

        public static Hotel CreateNew(string name)
        {
            return new Hotel(Guid.NewGuid(), name);
        }

        public Room AddRoom(string roomNumber)
        {
            if (_rooms is null) throw new PropertyNotInitializedException(nameof(Rooms));

            if (_rooms.Any(e => e.RoomNumber == roomNumber))
                throw new RoomAlreadyExist(roomNumber);

            var room = Room.CreateNew(roomNumber, this);
            _rooms.Add(room);

            return room;
        }

        public MessageTemplate AddMessageTemplate(string body, MessageType type,
            CommunicationMethod communicationMethod)
        {
            if (_messageTemplates is null) throw new PropertyNotInitializedException(nameof(MessageTemplates));

            if (MessageTemplates.Any(m => m.MessageType == type && m.CommunicationMethod == communicationMethod))
                throw new MessageTemplateAlreadyExist();

            var template = MessageTemplate.CreateNew(body, this, type, communicationMethod);
            _messageTemplates.Add(template);
            return template;
        }
    }
}
