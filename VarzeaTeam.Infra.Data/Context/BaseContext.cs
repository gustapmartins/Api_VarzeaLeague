using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Collections.Immutable;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Model.DatabaseSettings;

namespace VarzeaLeague.Infra.Data.Context;

public abstract class BaseContext<T> where T : IEntity
{
    protected readonly IMongoCollection<T> Collection;

    protected BaseContext(IOptions<VarzeaLeagueDatabaseSettings> options, string collectionName)
    {
        var client = new MongoClient(options.Value.ConnectionString);
        var database = client.GetDatabase(options.Value.DatabaseName);
        Collection = database.GetCollection<T>(collectionName);
    }

    public async Task<IEnumerable<T>> GetAsync(int page, int pageSize, FilterDefinition<T> filter)
    {
        int skip = (page - 1) * pageSize;

        FindOptions<T> options = new()
        {
            Limit = pageSize,
            Skip = skip,
            Sort = Builders<T>.Sort.Descending(x => x.DateCreated) // Ordena por data de criação no próprio banco de dados
        };

        if (filter == null)
        {
            return await Collection.FindSync(_ => true, options).ToListAsync();
        }
        else
            return await Collection.FindSync(filter, options).ToListAsync();
    }


    public async Task CreateAsync(T addObject)
    {
        await Collection.InsertOneAsync(addObject);
    }

    public async Task<T> GetIdAsync(string Id)
    {
        return await Collection.Find(x => x.Id == Id).FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(string Id)
    {
        await Collection.DeleteOneAsync(x => x.Id == Id);
    }
}