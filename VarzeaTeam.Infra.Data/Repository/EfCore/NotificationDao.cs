using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Infra.Data.Context;


namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class NotificationDao : BaseContext<NotificationModel>, INotificationDao
{

    private readonly IMongoCollection<NotificationModel> _NotificationCollection;

    public NotificationDao(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "NotificationCollection")
    {
        _NotificationCollection = Collection;
    }

    public Task CreateAsync(INotificationDao addObject)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<INotificationDao>> GetAsync(int page, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<INotificationDao> GetIdAsync(string Id)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string Id)
    {
        throw new NotImplementedException();
    }

    public Task<INotificationDao> UpdateAsync(string Id, INotificationDao updateObject)
    {
        throw new NotImplementedException();
    }
}
