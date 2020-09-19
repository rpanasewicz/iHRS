using System.Threading;
using System.Threading.Tasks;

namespace iHRS.Application.Common
{
    public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        Task<TResult> Handle(TCommand cmd);
    }

    public interface ICommandHandler<in TCommand> where TCommand : class, ICommand
    {
        Task Handle(TCommand cmd);
    }
}