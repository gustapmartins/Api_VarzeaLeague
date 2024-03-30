using Microsoft.Extensions.Options;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using MongoDB.Driver;

namespace VarzeaLeague.Infra.Data.Context;

public abstract class BaseContext<T>
{
    protected readonly IMongoCollection<T> Collection;

    protected BaseContext(IOptions<VarzeaLeagueDatabaseSettings> options, string collectionName)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        Collection = database.GetCollection<T>(collectionName);
    }
}