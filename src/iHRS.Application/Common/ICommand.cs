namespace iHRS.Application.Common
{
    public interface ICommand : ICommand<Unit>
    {
    }

    public interface ICommand<TResult>
    {

    }
}
