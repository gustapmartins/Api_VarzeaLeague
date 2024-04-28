using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Dao;

public interface INotificationDao
{
    Task<IEnumerable<NotificationModel>> GetAsync();

    Task<NotificationModel> GetIdAsync(string Id);

    Task CreateAsync(NotificationModel addObject);

    Task RemoveAsync(string Id);
}
