using iHRS.Domain.DomainEvents.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Decorators
{
    internal sealed class DomainEventLoggingDecorator<TDomainEvent> : IDomainEventHandler<TDomainEvent> where TDomainEvent : class, IDomainEvent
    {
        private readonly IDomainEventHandler<TDomainEvent> _handler;
        private readonly ILogger<TDomainEvent> _logger;

        public DomainEventLoggingDecorator(IDomainEventHandler<TDomainEvent> handler, ILogger<TDomainEvent> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task Handle(TDomainEvent domainEvent)
        {
            try
            {
                _logger.LogInformation("Handling domain event ({domainEventName}) by ({handlerName}).", domainEvent.GetType().Name, _handler.GetType().Name);
                _logger.LogDebug("Handling domain event ({domainEventName}) by ({handlerName}). ({@domainEvent})", domainEvent.GetType().Name, _handler.GetType().Name, domainEvent);

                await _handler.Handle(domainEvent);

                _logger.LogInformation("Handling {domainEventName} finished.", domainEvent.GetType().Name);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling {domainEventName}!", domainEvent.GetType().Name);
                throw;
            }
        }
    }
}