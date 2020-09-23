using System.Threading.Tasks;

namespace iHRS.Application.Services
{
    public interface IMessageService : IService
    {
        Task SendMessage(string message, string[] recipients, string connectionType);
    }
}
