using iHRS.Domain.DomainEvents.Abstractions;
using System;
using System.Collections.Generic;

namespace iHRS.Domain.Common
{
    public class Entity
    {
        public Guid Id { get; protected set; }

        public string CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public DateTime? ExpirationDate { get; set; }

        private readonly List<IDomainEvent> _events = new List<IDomainEvent>();
        public IEnumerable<IDomainEvent> Events => _events;

        protected void AddEvent(IDomainEvent @event)
        {
            _events.Add(@event);
        }

        public void ClearEvents() => _events.Clear();
    }
}