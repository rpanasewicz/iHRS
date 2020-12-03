using System.Threading.Tasks;

namespace iHRS.Domain.DomainEvents.Abstractions
{
    public interface IDomainEventDispatcher
    {
        Task PublishAsync<T>(T @event) where T : class, IDomainEvent;
    }
}
