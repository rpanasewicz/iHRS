using iHRS.Application.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Decorators
{
    internal sealed class CommandHandlerLoggingDecorator<TCommand, TResult> : ICommandHandler<TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        private readonly ICommandHandler<TCommand, TResult> _handler;
        private readonly ILogger<TCommand> _logger;

        public CommandHandlerLoggingDecorator(ICommandHandler<TCommand, TResult> handler, ILogger<TCommand> logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public async Task<TResult> Handle(TCommand command)
        {
            try
            {
                _logger.LogInformation("Handling command - {commandName}.", command.GetType().Name);
                _logger.LogDebug("Handling command - {commandName}. ({@command})", command.GetType().Name, command);

                var result = await _handler.Handle(command);

                _logger.LogInformation("Handling {commandName} finished.", command.GetType().Name);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "ERROR Handling {commandName}!", command.GetType().Name);
                throw;
            }
        }
    }
}