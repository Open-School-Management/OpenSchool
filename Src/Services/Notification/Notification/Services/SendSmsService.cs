using System.Text.RegularExpressions;
using Notification.TwilioHelper;
using Twilio.Rest.Api.V2010.Account;

namespace Notification.Services;

public class SendSmsService : ISendSmsService
{
    private readonly ITwilioService _twilioService;
    private readonly ILogger<SendSmsService> _logger;
    
    public SendSmsService(ITwilioService twilioService, ILogger<SendSmsService> logger)
    {
        _twilioService = twilioService;
        _logger = logger;
    }

    public async Task SendAsync(string to, string message, CancellationToken cancellationToken = default)
    {
        string phone = FormatPhone(to);
        if (!IsValidPhoneNumber(phone))
        {
            _logger.LogError("Invalid phone number: {PhoneNumber}. Please provide a valid phone number.", to);
            return;
        }

        message = FormatMessage(message);
        var response = await _twilioService.SendAsync(phone, message);

        if (response.ErrorCode.HasValue)
        {
            _logger.LogError("Twilio error: {ErrorCode} - {ErrorMessage}", response.ErrorCode, response.ErrorMessage);
        }
        else
        {
            _logger.LogInformation("Message sent successfully to {PhoneNumber}.", to);
        }
        
    }

    private string FormatPhone(string phone)
    {
        phone = Regex.Replace(phone, @"[^\d]", "");
        
        if (!phone.StartsWith("+84"))
        {
            if (phone.StartsWith("0"))
            {
                phone = phone.Substring(1);
            }
            phone = "+84" + phone;
        }
    
        return phone;
    }

    private string FormatMessage(string message)
    {
        return $"OS-{message} là mã xác nhận Open School của bạn";
    }
    
    private bool IsValidPhoneNumber(string phone)
    {
        var regex = new Regex(@"^\+84([3|5|7|8|9])([0-9]{8})\b");
        return regex.IsMatch(phone);
    }
}