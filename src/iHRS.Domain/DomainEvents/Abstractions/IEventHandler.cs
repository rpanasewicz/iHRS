using System.Threading.Tasks;

namespace iHRS.Domain.DomainEvents.Abstractions
{
    public interface IEventHandler<in TEvent> where TEvent : class, IDomainEvent
    {
        Task HandleAsync(TEvent @event);
    }
}