using Twilio.Rest.Api.V2010.Account;

namespace Notification.TwilioHelper;

public interface ITwilioService
{
    Task<MessageResource> SendAsync(string to, string message);
}