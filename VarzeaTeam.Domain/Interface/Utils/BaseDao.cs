using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Infra.Data.Repository.Utils;

public interface BaseDao<T>
{
    Task<List<T>> GetAsync(int page, int pageSize);

    Task<T> GetIdAsync(string Id);

    Task CreateAsync(T addObject);

    Task RemoveAsync(string Id);

    Task<T> UpdateAsync(string Id, T updateObject);
}
