using Core.Mailing.Models;

namespace Core.Mailing.Abstractions;

public interface IMailService
{
    void SendMail(Mail mail);
    Task SendEmailAsync(Mail mail);
}
