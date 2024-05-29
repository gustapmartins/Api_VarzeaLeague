using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface ITeamService
{
    Task<IEnumerable<TeamModel>> GetAsync(int page, int pageSize);

    Task<TeamModel> GetIdAsync(string Id);

    Task<TeamModel> GetNameAsync(string NameTeam);

    Task<TeamModel> CreateAsync(TeamModel addObject);

    Task<TeamModel> RemoveAsync(string Id);

    Task<TeamModel> UpdateAsync(string Id, TeamModel updateObject);
}
