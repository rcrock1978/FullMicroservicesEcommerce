using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NotificationService.Application.Services;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace NotificationService.Infrastructure.Services;

public class EmailService : IEmailService
{
    private readonly string _sendGridApiKey;
    private readonly string _fromEmail;
    private readonly string _fromName;
    private readonly ILogger<EmailService> _logger;

    public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
    {
        _sendGridApiKey = configuration["SendGrid:ApiKey"] 
            ?? throw new ArgumentException("SendGrid ApiKey not configured");
        _fromEmail = configuration["SendGrid:FromEmail"] ?? "noreply@ecommerce.com";
        _fromName = configuration["SendGrid:FromName"] ?? "E-Commerce Platform";
        _logger = logger;
    }

    public async Task<bool> SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new SendGridClient(_sendGridApiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail);
            var msg = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            
            var response = await client.SendEmailAsync(msg, cancellationToken);
            
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Email sent successfully to {Email}", toEmail);
                return true;
            }

            _logger.LogWarning("Failed to send email to {Email}. Status: {Status}", toEmail, response.StatusCode);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending email to {Email}", toEmail);
            return false;
        }
    }

    public async Task<bool> SendTemplateEmailAsync(string toEmail, string templateId, Dictionary<string, string> templateData, CancellationToken cancellationToken = default)
    {
        try
        {
            var client = new SendGridClient(_sendGridApiKey);
            var msg = new SendGridMessage
            {
                From = new EmailAddress(_fromEmail, _fromName),
                TemplateId = templateId
            };
            msg.AddTo(new EmailAddress(toEmail));
            msg.SetTemplateData(templateData);

            var response = await client.SendEmailAsync(msg, cancellationToken);
            return response.IsSuccessStatusCode;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending template email to {Email}", toEmail);
            return false;
        }
    }
}
