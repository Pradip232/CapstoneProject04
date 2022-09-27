using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace eTraveller.Services
{
    public class MyEmailSenderService : IEmailSender
    {
        private readonly ILogger<MyEmailSenderService> _logger;

        public MyEmailSenderService(ILogger<MyEmailSenderService> logger)
        {
            _logger = logger;
        }
        #region IEmailSender Member

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            _logger.LogInformation($"Email sent to: {email}, Content: {htmlMessage}");
            return Task.CompletedTask;
        
        }
        #endregion
    }
}
