using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Infra.Data.Context;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class NotificationEfDao : BaseContext<NotificationModel>, INotificationDao
{

    private readonly IMongoCollection<NotificationModel> _NotificationCollection;

    public NotificationEfDao(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "NotificationCollection")
    {
        _NotificationCollection = Collection;
    }
}
