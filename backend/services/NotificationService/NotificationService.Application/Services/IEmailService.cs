namespace NotificationService.Application.Services;

public interface IEmailService
{
    Task<bool> SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default);
    Task<bool> SendTemplateEmailAsync(string toEmail, string templateId, Dictionary<string, string> templateData, CancellationToken cancellationToken = default);
}
