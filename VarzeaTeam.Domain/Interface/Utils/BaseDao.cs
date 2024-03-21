namespace VarzeaTeam.Infra.Data.Repository.Utils;

public interface BaseDao<T>
{
    Task<List<T>> GetAsync();

    Task<T> GetIdAsync(string Id);

    Task CreateAsync(T addObject);

    Task RemoveAsync(string Id);

    Task UpdateAsync(string Id, T updateObject);
}
