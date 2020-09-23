using iHRS.Application.Common;
using System;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Dispatchers
{
    internal sealed class CommandDispatcher : ICommandDispatcher
    {
        private readonly IServiceProvider _serviceProvider;

        public CommandDispatcher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command)
        {
            var handlerType = typeof(ICommandHandler<,>).MakeGenericType(command.GetType(), typeof(TResponse));
            var handler = _serviceProvider.GetService(handlerType);
            var method = handlerType.GetMethod("Handle");

            return await (Task<TResponse>)method.Invoke(handler, new object[] { command });
        }
    }
}