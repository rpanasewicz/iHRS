using iHRS.Domain.DomainEvents.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Dispatchers
{
    internal sealed class DomainEventDispatcher : IDomainEventPublisher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public DomainEventDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task PublishAsync<T>(T @event) where T : class, IDomainEvent
        {
            using var scope = _serviceFactory.CreateScope();
            var handlers = scope.ServiceProvider.GetServices<IEventHandler<T>>();
            foreach (var handler in handlers)
            {
                await handler.HandleAsync(@event);
            }
        }
    }
}