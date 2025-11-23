namespace CleanArchitectureApi.Application.Contracts.Infrastructure;

/// <summary>
/// Service interface for email operations
/// </summary>
public interface IEmailService
{
    Task SendEmailAsync(string to, string subject, string body, bool isHtml = false);
    Task SendEmailWithAttachmentAsync(string to, string subject, string body, byte[] attachment, string attachmentName, bool isHtml = false);
    Task SendEmailWithTemplateAsync(string to, string templateName, Dictionary<string, object> templateData);

    // TODO: Add additional method signatures as needed
    // Task SendBulkEmailAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = false);
    // Task SendEmailWithBccAsync(List<string> to, List<string> bcc, string subject, string body, bool isHtml = false);
}