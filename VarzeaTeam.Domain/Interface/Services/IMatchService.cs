using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IMatchService
{
    Task<IEnumerable<MatchModel>> GetAsync(int page, int pageSize, FilterTypeEnum? FilterType = null, string? NameTeam = null,  DateTime? Date = null);

    Task<MatchModel> GetIdAsync(string Id);

    Task<MatchModel> CreateAsync(MatchModel addObject);

    Task<MatchModel> RemoveAsync(string Id);

    Task<MatchModel> UpdateAsync(string Id, MatchModel updateObject);
}