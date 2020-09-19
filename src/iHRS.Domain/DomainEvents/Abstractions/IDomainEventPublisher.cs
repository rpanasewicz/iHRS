using System.Threading.Tasks;

namespace iHRS.Domain.DomainEvents.Abstractions
{
    public interface IDomainEventPublisher
    {
        Task PublishAsync<T>(T @event) where T : class, IDomainEvent;
    }
}
