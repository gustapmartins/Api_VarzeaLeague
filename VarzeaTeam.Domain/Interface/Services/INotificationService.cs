using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationModel>> GetNotificationAsync(string idUser);

    Task<NotificationModel> SendNotificationAsync(NotificationModel NotificationModel);

    Task<NotificationModel> DeleteNotificationAsync(string idNotification);
}
