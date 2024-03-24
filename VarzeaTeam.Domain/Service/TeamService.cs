using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Service;

public class TeamService : ITeamService
{
    private readonly ITeamDao _teamDao;

    public TeamService(ITeamDao teamDao)
    {
        _teamDao = teamDao;
    }

    public async Task<TeamModel> CreateAsync(TeamModel addObject)
    {
        await _teamDao.CreateAsync(addObject);

        return addObject;
    }

    public async Task<List<TeamModel>> GetAsync()
    {
        return await _teamDao.GetAsync();
    }

    public async Task<TeamModel> GetIdAsync(string Id)
    {
        return await _teamDao.GetIdAsync(Id);
    }

    public async Task<TeamModel> RemoveAsync(string Id)
    {
        var findId = await _teamDao.GetIdAsync(Id);

        await _teamDao.RemoveAsync(Id);

        return findId;
    }

    public async Task<TeamModel> UpdateAsync(string Id, TeamModel updateObject)
    {
        var findId = await _teamDao.GetIdAsync(Id);

        await _teamDao.UpdateAsync(Id, updateObject);

        return findId;
    }
}
