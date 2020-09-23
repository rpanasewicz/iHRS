using iHRS.Application.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace iHRS.Infrastructure.Services
{
    internal class LoggingMessagingService : IMessageService
    {
        private readonly ILogger<LoggingMessagingService> _logger;

        public LoggingMessagingService(ILogger<LoggingMessagingService> logger)
        {
            _logger = logger;
        }

        public Task SendMessage(string message, string[] recipients, string connectionType)
        {
            _logger.LogInformation(
                "IMessageService.{method} called. (message: '{message}', recipients: {@recipients}, connectionType: '{connectionType}'",
                nameof(SendMessage), message, recipients, connectionType);

            return Task.CompletedTask; ;
        }
    }
}
