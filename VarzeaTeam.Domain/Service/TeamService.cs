using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaTeam.Domain.Model.Team;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaTeam.Service;

public class TeamService : ITeamService
{
    private readonly ITeamDao _teamDao;

    public TeamService(ITeamDao teamDao)
    {
        _teamDao = teamDao;
    }

    public async Task<List<TeamModel>> GetAsync()
    {
        try 
        {
            List<TeamModel> GetAll = await _teamDao.GetAsync();

            if(GetAll.Count == 0)
                throw new ExceptionFilter($"Não existe nenhum time cadastrado");

            return GetAll;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<TeamModel> GetIdAsync(string Id)
    {
        try
        {
            TeamModel getId = await _teamDao.GetIdAsync(Id);

            if(getId == null)
                throw new ExceptionFilter($"O Time com o id '{Id}', não existe.");

            return getId;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<TeamModel> CreateAsync(TeamModel addObject)
    {
        try
        {
            TeamModel teamExist = await _teamDao.TeamExist(addObject.NameTeam);

            if(teamExist != null)
                throw new ExceptionFilter($"O Time com o nome '{addObject.NameTeam}', já existe.");

            await _teamDao.CreateAsync(addObject);
            return addObject;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<TeamModel> RemoveAsync(string Id)
    {
        try
        {
            var findId = await GetIdAsync(Id);

            await _teamDao.RemoveAsync(Id);

            return findId;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<TeamModel> UpdateAsync(string Id, TeamModel updateObject)
    {
        try
        {
            TeamModel findTeam = await GetIdAsync(Id);

            TeamModel updateTeam = await _teamDao.UpdateAsync(Id, updateObject);

            return updateTeam;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
