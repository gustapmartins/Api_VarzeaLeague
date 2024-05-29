using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Utils;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Domain.Service;

public class MatchService : IMatchService
{
    private readonly IMatchDao _matchDao;
    private readonly ITeamService _teamService;
    private readonly INotificationService _notificationService;

    public MatchService(IMatchDao matchDao, ITeamService teamService, INotificationService notificationService)
    {
        _matchDao = matchDao;
        _teamService = teamService;
        _notificationService = notificationService;
    }

    public async Task<IEnumerable<MatchModel>> GetAsync(int page, int pageSize)
    {
        try
        {
            IEnumerable<MatchModel> GetAll = await _matchDao.GetAsync(page, pageSize);

            if (GetAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhuma partida cadastrada");

            return GetAll;
        }
        catch (ExceptionFilter ex) 
        {
            throw new ExceptionFilter(ex.Message);
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
        catch (ExceptionFilter ex) 
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<MatchModel> CreateAsync(MatchModel addObject)
    {
        try
        {
            // Verificar se os times foram encontrados
            TeamModel homeTeam = await _teamService.GetNameAsync(addObject.HomeTeamName);
            TeamModel visitingTeam = await _teamService.GetNameAsync(addObject.VisitingTeamName);
            MatchModel matchExist = await _matchDao.MatchExistsAsync(homeTeam.Id, visitingTeam.Id);

            if (matchExist != null)
                throw new ExceptionFilter("Já existe uma partida cadastrada com esses times");

            bool andressExist = await ViaCep.GetCep(addObject.Local);

            if(!andressExist)
            {
                throw new ExceptionFilter("Esse endereço não existe");
            }

            // Criar a partida com os times encontrados
            MatchModel match = new()
            {
                HomeTeamModel = homeTeam,
                HomeTeamName = homeTeam.NameTeam,
                VisitingTeamModel = visitingTeam,
                VisitingTeamName = visitingTeam.NameTeam,
                Local = addObject.Local,
                Date = addObject.Date,
                DateCreated = DateTime.Now,
            };

            // Salvar a partida no banco de dados
            await _matchDao.CreateAsync(match);

            NotificationModel notification = new()
            {
                UserHomeId = homeTeam.ClientId,
                UserVisitingId = visitingTeam.ClientId,
                NotificationType = "Agendamento de partida de jogo",
                DateCreated = DateTime.Now,
            };

            await _notificationService.SendNotificationAsync(notification);

            return match;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<MatchModel> RemoveAsync(string Id)
    {
        try
        {
            MatchModel findId = await GetIdAsync(Id);

            await _matchDao.RemoveAsync(Id);

            return findId;
        }
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<MatchModel> UpdateAsync(string Id, MatchModel updateObject)
    {
        try
        {
            MatchModel findId = await GetIdAsync(Id);

            var updateFields = new Dictionary<string, object>
            {
                 { $"{nameof(updateObject.HomeTeamModel)}.{nameof(updateObject.HomeTeamModel.NameTeam)}", updateObject.HomeTeamModel.NameTeam },
                 { $"{nameof(updateObject.VisitingTeamModel)}.{nameof(updateObject.VisitingTeamModel.NameTeam)}", updateObject.VisitingTeamModel.NameTeam },
                 { nameof(updateObject.Local), updateObject.Local },
                 { nameof(updateObject.TeamWin), updateObject.TeamWin },
                 { nameof(updateObject.Date), updateObject.Date }
                // Adicione outros campos que deseja atualizar conforme necessário
            };

            MatchModel matchUpdate = await _matchDao.UpdateAsync(Id, updateFields);

            return matchUpdate;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }
}
