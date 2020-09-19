using System.Threading.Tasks;
using iHRS.Application.Common;
using Microsoft.Extensions.DependencyInjection;

namespace iHRS.Infrastructure.Dispatchers
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceScopeFactory _serviceFactory;

        public CommandDispatcher(IServiceScopeFactory serviceFactory)
        {
            _serviceFactory = serviceFactory;
        }

        public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command)
        {
            using var scope = _serviceFactory.CreateScope();

            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            var handler = scope.ServiceProvider.GetService(handlerType);
            var method = handlerType.GetMethod("Handle");

            return (TResponse)await ((Task<TResponse>)method?.Invoke(handler, new[] { command }));
        }

        public async Task SendAsync(ICommand command)
        {
            using var scope = _serviceFactory.CreateScope();
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(command.GetType());
            var handler = scope.ServiceProvider.GetService(handlerType);

            var method = handlerType.GetMethod("Handle");

            await (Task)method.Invoke(handler, new object[] { command });
        }
    }
}