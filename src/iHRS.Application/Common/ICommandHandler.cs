using System.Threading.Tasks;

namespace iHRS.Application.Common
{
    public interface ICommandHandler<in TCommand, TResult> where TCommand : class, ICommand<TResult>
    {
        Task<TResult> Handle(TCommand cmd);
    }

    public interface ICommandHandler<in TCommand> : ICommandHandler<TCommand, Unit> where TCommand : class, ICommand<Unit>
    {

    }
}