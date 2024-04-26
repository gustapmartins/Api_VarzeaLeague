using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IMatchService
{
    Task<IEnumerable<MatchModel>> GetAsync(int page, int pageSize);

    Task<MatchModel> GetIdAsync(string Id);

    Task<MatchModel> CreateAsync(MatchModel addObject);

    Task<MatchModel> RemoveAsync(string Id);

    Task<MatchModel> UpdateAsync(string Id, MatchModel updateObject);
}