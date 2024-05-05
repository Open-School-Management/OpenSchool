using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace Notification.TwilioHelper;

public class TwilioService : ITwilioService
{
    private readonly IOptions<TwilioSettings> _options;

    public TwilioService(IOptions<TwilioSettings> options)
    {
        _options = options ?? throw new ArgumentNullException(nameof(options));
        TwilioClient.Init(options.Value.AccountSID, options.Value.AuthToken);
    }
    
    public async Task<MessageResource> SendAsync(string to, string message)
    {
        
         var response = await MessageResource.CreateAsync(
            body: message,
            from: new  Twilio.Types.PhoneNumber(_options.Value.PhoneNumber),
            to: new Twilio.Types.PhoneNumber(to)
        );

        return response;

    }
}