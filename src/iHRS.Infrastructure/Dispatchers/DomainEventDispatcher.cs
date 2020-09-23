using iHRS.Domain.DomainEvents.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Dispatchers
{
    internal sealed class DomainEventDispatcher : IDomainEventPublisher
    {
        private readonly IServiceProvider _serviceProvider;

        public DomainEventDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task PublishAsync<T>(T @event) where T : class, IDomainEvent
        {
            var handlerType = typeof(IDomainEventHandler<>).MakeGenericType(@event.GetType());
            var handlers = _serviceProvider.GetServices(handlerType);
            var method = handlerType.GetMethod("Handle");

            foreach (var handler in handlers)
            {
                await ((Task)method?.Invoke(handler, new object[] { @event }) ?? Task.CompletedTask);
            }
        }
    }
}