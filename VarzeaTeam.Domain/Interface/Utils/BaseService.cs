namespace VarzeaLeague.Domain.Interface.Utils;

public interface BaseService<T>
{
    Task<List<T>> GetAsync(int page, int pageSize);

    Task<T> GetIdAsync(string Id);

    Task<T> CreateAsync(T addObject);

    Task<T> RemoveAsync(string Id);

    Task<T> UpdateAsync(string Id, T updateObject);
}
