using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Application.DTO.Notification;

public class NotificationViewDto
{
    public string Id { get; set; } = string.Empty;

    public string UserHomeId { get; set; } = string.Empty;

    public string UserVisitingId { get; set; } = string.Empty;

    public string NotificationType { get; set; } = string.Empty;
 
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
