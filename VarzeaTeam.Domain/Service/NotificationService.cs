using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Service;

public class NotificationService : INotificationService
{
    public NotificationService()
    {

    }

    public Task DeleteNotificationAsync(string idNotification)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<NotificationModel>> GetNotificationAsync(string idUser)
    {
        throw new NotImplementedException();
    }

    public Task<NotificationModel> SendNotificationAsync(NotificationModel NotificationModel)
    {
        throw new NotImplementedException();
    }
}
