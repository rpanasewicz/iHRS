using System.Threading.Tasks;

namespace iHRS.Application.Common
{
    public interface ICommandDispatcher
    {
        Task<TResponse> SendAsync<TResponse>(ICommand<TResponse> command);
        Task SendAsync(ICommand command);
    }
}