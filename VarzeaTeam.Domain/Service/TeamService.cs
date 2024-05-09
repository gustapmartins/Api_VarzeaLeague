using MongoDB.Driver;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaTeam.Service;

public class TeamService : ITeamService
{
    private readonly ITeamDao _teamDao;

    public TeamService(ITeamDao teamDao)
    {
        _teamDao = teamDao;
    }

    public async Task<IEnumerable<TeamModel>> GetAsync(int page, int pageSize)
    {
        try 
        {
            IEnumerable<TeamModel> GetAll = await _teamDao.GetAsync(page, pageSize, 
                filter: Builders<TeamModel>.Filter.Where(x => x.Active == true));

            if(GetAll.Count() == 0)
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
