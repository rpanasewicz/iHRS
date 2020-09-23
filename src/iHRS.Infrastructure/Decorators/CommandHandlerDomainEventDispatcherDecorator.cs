using iHRS.Application.Common;
using iHRS.Domain.Common;
using iHRS.Domain.DomainEvents.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Decorators
{
    internal sealed class CommandHandlerDomainEventDispatcherDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;
        private readonly ILogger<TCommand> _logger;
        private readonly HRSContext _context;
        private readonly IDomainEventPublisher _domainEventPublisher;

        public CommandHandlerDomainEventDispatcherDecorator(ICommandHandler<TCommand, TResult> handler,
            ILogger<TCommand> logger,
            HRSContext context,
            IDomainEventPublisher domainEventPublisher)
        {
            _handler = handler;
            _logger = logger;
            _context = context;
            _domainEventPublisher = domainEventPublisher;
        }

        public async Task<TResult> Handle(TCommand command)
        {
            var result = await _handler.Handle(command);

            var domainEntities = _context.ChangeTracker
                .Entries<Entity>()
                .Where(x => x.Entity.Events != null && x.Entity.Events.Any())
                .ToList();

            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.Events)
                .ToList();

            domainEntities
                .ForEach(entity => entity.Entity.ClearEvents());

            foreach (var domainEvent in domainEvents)
            {
                _logger.LogInformation("Publishing domain event {domainEventType}.", domainEvent.GetType().Name);
                _logger.LogDebug("Publishing domain event {domainEventType} ({@domainEvent}).", domainEvent.GetType().Name, domainEvent);
                await _domainEventPublisher.PublishAsync(domainEvent);
            }

            return result;
        }
    }
}