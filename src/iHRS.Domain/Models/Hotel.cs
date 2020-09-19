using iHRS.Domain.Common;
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

        private ISet<Room> _rooms;

        private Hotel() { } // For EF

        private Hotel(Guid id, string name, IEnumerable<Room> rooms = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(name));

            Id = id;
            Name = name;
            Rooms = rooms ?? Enumerable.Empty<Room>();
        }

        public static Hotel CreateNew(string name)
        {
            return new Hotel(Guid.NewGuid(), name);
        }
    }
}
