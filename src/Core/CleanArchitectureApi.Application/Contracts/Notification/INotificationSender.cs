namespace CleanArchitectureApi.Application.Contracts.Notification;

/// <summary>
/// Service interface for date and time operations
/// </summary>
public interface INotificationSender
{    
    void SendNotification(string message);

    // TODO: Add additional method signatures as needed
    // Task SendBulkNotificationAsync(IEnumerable<string> recipients, string subject, string body, bool isHtml = false)
}