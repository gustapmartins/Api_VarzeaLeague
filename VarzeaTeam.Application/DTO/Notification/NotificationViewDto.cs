using MongoDB.Bson.Serialization.Attributes;
using System.Text.Json.Serialization;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Application.DTO.Notification;

public class NotificationViewDto
{

    [JsonPropertyName("Id")]
    public string Id { get; set; } = string.Empty;

    [JsonPropertyName("UserHomeTeamModel")]
    public TeamModel? UserHomeTeamModel { get; set; }

    [JsonPropertyName("UserVisitingTeamModel")]
    public TeamModel? UserVisitingTeamModel { get; set; }

    [JsonPropertyName("NotificationType")]

    public string NotificationType { get; set; } = string.Empty;

    [JsonPropertyName("ReadNotification")]
    public bool ReadNotification { get; set; }

    [JsonPropertyName("DateCreated")]
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
