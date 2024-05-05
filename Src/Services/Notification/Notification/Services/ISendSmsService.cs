using Twilio.Rest.Api.V2010.Account;

namespace Notification.Services;

public interface ISendSmsService
{
    Task SendAsync(string to, string message, CancellationToken cancellationToken = default);
}