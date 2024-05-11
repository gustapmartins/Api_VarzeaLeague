using MongoDB.Driver;
using VarzeaLeague.Domain.Model;

namespace VarzeaTeam.Infra.Data.Repository.Utils;

public interface BaseDao<T>
{
    Task<IEnumerable<T>> GetAsync(int page, int pageSize, FilterDefinition<T>? filter = null);

    Task<T> GetIdAsync(string Id);

    Task CreateAsync(T addObject);

    Task RemoveAsync(string Id);

    Task<T> UpdateAsync(string Id, IDictionary<string, object> updateFields);
}
