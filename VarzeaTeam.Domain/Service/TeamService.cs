using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.JwtHelper;
using VarzeaTeam.Domain.Exceptions;
using VarzeaLeague.Domain.Model;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace VarzeaTeam.Service;

public class TeamService : ITeamService
{
    private readonly ITeamDao _teamDao;
    private readonly HttpContext _httpContext;

    public TeamService(ITeamDao teamDao, IHttpContextAccessor httpContextAccessor)
    {
        _teamDao = teamDao;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<IEnumerable<TeamModel>> GetAsync(int page, int pageSize)
    {
        try 
        {
            IEnumerable<TeamModel> GetAll = await _teamDao.GetAsync(page, pageSize, 
                filter: Builders<TeamModel>.Filter.Where(x => x.Active == true));

            if (!GetAll.Any())
                throw new ExceptionFilter("Não existe nenhum time cadastrado");

            return GetAll;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message); // Se não, transforma em uma ExceptionFilter
        }
    }

    public async Task<TeamModel> GetIdAsync(string Id)
    {
        try
        {
            TeamModel getId = await _teamDao.GetIdAsync(Id);

            if(getId == null)
                throw new ExceptionFilter($"O Time com o id {Id}, não existe.");

            return getId;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message); // Se não, transforma em uma ExceptionFilter
        }
    }

    public async Task<TeamModel> CreateAsync(TeamModel addObject)
    {
        try
        {
            TeamModel teamExist = await _teamDao.TeamExist(addObject.NameTeam);

            if(teamExist != null)
                throw new ExceptionFilter($"O Time com o nome '{addObject.NameTeam}', já existe.");

            //Visualização através do JWT authenticado na aplicação
            string clientId = GetTokenId.GetClientIdFromToken(_httpContext);

            addObject.clientId = clientId;
            await _teamDao.CreateAsync(addObject);

            return addObject;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
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
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<TeamModel> UpdateAsync(string Id, TeamModel updateObject)
    {
        try
        {
            TeamModel findTeam = await GetIdAsync(Id);

            var updateFields = new Dictionary<string, object>
            {
                { nameof(updateObject.NameTeam), updateObject.NameTeam },
                { nameof(updateObject.Active), updateObject.Active }
                // Adicione outros campos que deseja atualizar conforme necessário
            };

            TeamModel updateTeam = await _teamDao.UpdateAsync(Id, updateFields);

            return updateTeam;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }
}
