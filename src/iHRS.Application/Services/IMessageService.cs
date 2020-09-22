using System.Threading.Tasks;

namespace iHRS.Application.Services
{
    public interface IMessageService
    {
        Task SendMessage(string message, string[] recipients, string connectionType);
    }
}
