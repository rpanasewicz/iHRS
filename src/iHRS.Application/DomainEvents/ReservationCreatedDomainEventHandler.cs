using System;
using System.Threading.Tasks;
using iHRS.Domain.DomainEvents;
using iHRS.Domain.DomainEvents.Abstractions;

namespace iHRS.Application.DomainEvents
{
    public class ReservationCreatedDomainEventHandler : IDomainEventHandler<ReservationCreatedDomainEvent>
    {
        public async Task HandleAsync(ReservationCreatedDomainEvent @event)
        {
            throw new NotImplementedException();
        }
    }
}
