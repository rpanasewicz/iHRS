using System.Threading.Tasks;

namespace iHRS.Domain.DomainEvents.Abstractions
{
    public interface IDomainEventHandler<in TEvent> where TEvent : class, IDomainEvent
    {
        Task Handle(TEvent domainEvent);
    }
}