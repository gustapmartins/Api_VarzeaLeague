using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface INotificationService
{
    Task<IEnumerable<NotificationModel>> GetNotificationAsync(int page, int pageSize);

    Task<NotificationModel> SendNotificationAsync(NotificationModel NotificationModel);
}
