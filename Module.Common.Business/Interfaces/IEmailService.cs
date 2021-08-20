using System.Threading.Tasks;
using KristaShop.Common.Models;

namespace Module.Common.Business.Interfaces {
    public interface IEmailService {
        Task SendEmailAsync(EmailMessage message, string recipientName);
    }
}