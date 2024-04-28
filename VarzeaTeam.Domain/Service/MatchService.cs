using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaTeam.Domain.Exceptions;
using VarzeaLeague.Domain.Utils;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Domain.Service;

public class MatchService : IMatchService
{
    private readonly IMatchDao _matchDao;
    private readonly ITeamService _teamService;

    public MatchService(IMatchDao matchDao, ITeamService teamService)
    {
        _matchDao = matchDao;
        _teamService = teamService;
    }

    public async Task<IEnumerable<MatchModel>> GetAsync(int page, int pageSize)
    {
        try
        {
            IEnumerable<MatchModel> GetAll = await _matchDao.GetAsync(page, pageSize, filter: null);

            if (GetAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhuma partida cadastrada");

            return GetAll;
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<MatchModel> GetIdAsync(string Id)
    {
        try
        {
            MatchModel GetId = await _matchDao.GetIdAsync(Id);

            if (GetId == null)
                throw new ExceptionFilter($"A partida com o id '{Id}', não existe.");

            return GetId;
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<MatchModel> CreateAsync(MatchModel addObject)
    {
        try
        {
            TeamModel homeTeam = await _teamService.GetIdAsync(addObject.HomeTeamId);
            TeamModel visitingTeam = await _teamService.GetIdAsync(addObject.VisitingTeamId);
            MatchModel matchExist = await _matchDao.MatchExistsAsync(addObject.HomeTeamId, addObject.VisitingTeamId);

            // Verificar se os times foram encontrados
            
            if (matchExist != null)
            {
                throw new ExceptionFilter("Já existe uma partida cadastrada com esses times.");
            }

            bool andressExist = await ViaCep.GetCep(addObject.Local);

            if(!andressExist)
            {
                throw new ExceptionFilter("Esse endereço não existe");
            }

            // Criar a partida com os times encontrados
            MatchModel match = new()
            {
                HomeTeamModel = homeTeam,
                HomeTeamId = addObject.HomeTeamId,
                VisitingTeamModel = visitingTeam,
                VisitingTeamId = addObject.VisitingTeamId,
                Local = addObject.Local,
                Date = addObject.Date,
                DateCreated = DateTime.Now,
            };

            // Salvar a partida no banco de dados
            await _matchDao.CreateAsync(match);

            return match;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<MatchModel> RemoveAsync(string Id)
    {
        try
        {
            MatchModel findId = await _matchDao.GetIdAsync(Id);

            await _matchDao.RemoveAsync(Id);

            return findId;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<MatchModel> UpdateAsync(string Id, MatchModel updateObject)
    {
        try
        {
            MatchModel findId = await GetIdAsync(Id);

            MatchModel matchUpdate = await _matchDao.UpdateAsync(Id, updateObject);

            return matchUpdate;
        }
        catch(Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }
}
