using iHRS.Application.Common;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

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

            return await (Task<TResponse>)method.Invoke(handler, new object[] { command });
        }
    }
}