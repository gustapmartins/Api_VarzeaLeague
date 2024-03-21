using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VarzeaLeague.Domain.Model.DatabaseSettings;

namespace VarzeaLeague.Infra.Data.Context;

public abstract class BaseContext<T>
{
    protected readonly IMongoCollection<T> Collection;

    protected BaseContext(IOptions<VarzeaLeagueDatabaseSettings> options)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        Collection = database.GetCollection<T>(options.Value.VarzeaLeagueCollectionName);
    }
}