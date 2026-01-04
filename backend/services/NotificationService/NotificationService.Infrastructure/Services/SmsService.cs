using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Services;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace NotificationService.Infrastructure.Services;

public class SmsService : ISmsService
{
    private readonly string _accountSid;
    private readonly string _authToken;
    private readonly string _fromPhoneNumber;
    private readonly ILogger<SmsService> _logger;

    public SmsService(IConfiguration configuration, ILogger<SmsService> logger)
    {
        _accountSid = configuration["Twilio:AccountSid"] 
            ?? throw new ArgumentException("Twilio AccountSid not configured");
        _authToken = configuration["Twilio:AuthToken"] 
            ?? throw new ArgumentException("Twilio AuthToken not configured");
        _fromPhoneNumber = configuration["Twilio:FromPhoneNumber"] 
            ?? throw new ArgumentException("Twilio FromPhoneNumber not configured");
        _logger = logger;

        TwilioClient.Init(_accountSid, _authToken);
    }

    public async Task<bool> SendSmsAsync(string phoneNumber, string message, CancellationToken cancellationToken = default)
    {
        try
        {
            var messageResource = await MessageResource.CreateAsync(
                to: new PhoneNumber(phoneNumber),
                from: new PhoneNumber(_fromPhoneNumber),
                body: message
            );

            if (messageResource.Status == MessageResource.StatusEnum.Sent || 
                messageResource.Status == MessageResource.StatusEnum.Queued)
            {
                _logger.LogInformation("SMS sent successfully to {PhoneNumber}", phoneNumber);
                return true;
            }

            _logger.LogWarning("Failed to send SMS to {PhoneNumber}. Status: {Status}", phoneNumber, messageResource.Status);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending SMS to {PhoneNumber}", phoneNumber);
            return false;
        }
    }
}
