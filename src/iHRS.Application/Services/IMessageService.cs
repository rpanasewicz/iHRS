using System.Threading.Tasks;

namespace iHRS.Application.Services
{
    public interface IMessageService : IService
    {
        Task SendMessage(string connectionType, string message, params string[] recipients);
    }
}
