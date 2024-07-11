using MongoDB.Driver;
using VarzeaLeague.Domain.Enum;
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

    public async Task<IEnumerable<MatchModel>> GetAsync(int page, int pageSize, FilterTypeEnum? FilterType = null, string? NameTeam = null, DateTime? Date = null)
     {
        try
        {
            FilterDefinition<MatchModel> filter;

            switch (FilterType)
            {
                case FilterTypeEnum.Ongoing:
                    filter = Builders<MatchModel>.Filter.And(
                        Builders<MatchModel>.Filter.Eq(m => m.TeamWin, string.Empty),
                        NameTeam == null ? Builders<MatchModel>.Filter.Empty : Builders<MatchModel>.Filter.Or(
                            Builders<MatchModel>.Filter.Eq(m => m.HomeTeamName, NameTeam),
                            Builders<MatchModel>.Filter.Eq(m => m.VisitingTeamName, NameTeam)
                        )
                    );
                    break;
                case FilterTypeEnum.Completed:
                    filter = Builders<MatchModel>.Filter.And(
                        Builders<MatchModel>.Filter.Ne(m => m.TeamWin, string.Empty),
                        NameTeam == null ? Builders<MatchModel>.Filter.Empty : Builders<MatchModel>.Filter.Or(
                            Builders<MatchModel>.Filter.Eq(m => m.HomeTeamName, NameTeam),
                            Builders<MatchModel>.Filter.Eq(m => m.VisitingTeamName, NameTeam)
                        )
                    );
                    break;
                case FilterTypeEnum.ByDate:
                    if (!Date.HasValue)
                    {
                        throw new ExceptionFilter("Data não fornecida para o filtro por data");
                    }

                    filter = Builders<MatchModel>.Filter.Gte(m => m.Date, Date.Value.Date) &
                                   Builders<MatchModel>.Filter.Lt(m => m.Date, Date.Value.Date.AddDays(1));
                    break;
                default:
                    filter = Builders<MatchModel>.Filter.Empty;
                    break;
            }

            IEnumerable<MatchModel> ReturnMatchAll = await _matchDao.GetAsync(page, pageSize, filter);

            if (!ReturnMatchAll.Any())
                throw new ExceptionFilter($"Não existe nenhuma partida cadastrada");

            return ReturnMatchAll;
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
                UserHomeTeamModel = homeTeam,
                UserVisitingTeamModel = visitingTeam,
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
            MatchModel existingMatch = await GetIdAsync(Id);

            // Verificar se os nomes dos times são iguais
            if (existingMatch.VisitingTeamName == updateObject.HomeTeamName || existingMatch.HomeTeamName == updateObject.VisitingTeamName)
            {
                throw new ExceptionFilter("O mesmo time não pode jogar contra si mesmo.");
            }

            Dictionary<string, object> updateFields = new()
            {
                { nameof(updateObject.HomeTeamName), updateObject.HomeTeamName ?? existingMatch.HomeTeamName },
                { nameof(updateObject.VisitingTeamName), updateObject.VisitingTeamName ?? existingMatch.VisitingTeamName },
                { nameof(updateObject.HomeTeamModel), updateObject.HomeTeamName == null ? existingMatch.HomeTeamModel : await _teamService.GetNameAsync(NameTeam: updateObject.HomeTeamName) },
                { nameof(updateObject.VisitingTeamModel), updateObject.VisitingTeamName == null ? existingMatch.VisitingTeamModel : await _teamService.GetNameAsync(NameTeam: updateObject.VisitingTeamName) },
                { nameof(updateObject.Local), updateObject.Local ?? existingMatch.Local },
                { nameof(updateObject.TeamWin), updateObject.TeamWin ?? existingMatch.TeamWin },
                { nameof(updateObject.Date), updateObject.Date != default ? updateObject.Date : existingMatch.Date }
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
