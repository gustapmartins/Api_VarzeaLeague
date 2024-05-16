using MongoDB.Driver;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Domain.Service;

public class PlayerService : IPlayerService
{
    private readonly IPlayerDao _playerDao;
    private readonly ITeamService _teamService;

    public PlayerService(IPlayerDao playerDao, ITeamService teamService)
    {
        _playerDao = playerDao;
        _teamService = teamService;
    }

    public async Task<IEnumerable<PlayerModel>> GetAsync(int page, int pageSize, string teamId)
    {
        try
        {
            TeamModel teamModel = await _teamService.GetIdAsync(teamId);

            IEnumerable<PlayerModel> playerAll = await _playerDao.GetAsync(page, pageSize, 
                filter: teamModel != null ? Builders<PlayerModel>.Filter.Where(x => x.TeamId == teamModel.Id) : null);

            if (playerAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhum player cadastrado");

            return playerAll;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<PlayerModel> GetIdAsync(string Id)
    {
        try
        {
            PlayerModel player = await _playerDao.GetIdAsync(Id);

            if (player == null)
                throw new ExceptionFilter($"O jogador com o id '{Id}' não existe.");

            return player;
        }
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<PlayerModel> CreateAsync(PlayerModel addObject)
    {
        try
        {
            PlayerModel existingPlayer = await _playerDao.PlayerExist(addObject.NamePlayer);

            if(existingPlayer != null)
                throw new ExceptionFilter($"O jogador com o nome '{addObject.NamePlayer}' já existe.");

            TeamModel team = await _teamService.GetIdAsync(addObject.TeamId);

            addObject.TeamId = team.Id;
            addObject.TeamModel = team;

            await _playerDao.CreateAsync(addObject);

            return addObject;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<PlayerModel> RemoveAsync(string Id)
    {
        try
        {
            PlayerModel findId = await GetIdAsync(Id);

            await _playerDao.RemoveAsync(Id);

            return findId;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<PlayerModel> UpdateAsync(string Id, PlayerModel updateObject)
    {
        try
        {
            PlayerModel findId = await _playerDao.GetIdAsync(Id);

            var updateFields = new Dictionary<string, object>
            {
                { nameof(updateObject.NamePlayer), updateObject.NamePlayer },
                { nameof(updateObject.Age), updateObject.Age },
                { nameof(updateObject.TeamId), updateObject.TeamId }
                // Adicione outros campos que deseja atualizar conforme necessário
            };

            PlayerModel updatePlayer = await _playerDao.UpdateAsync(Id, updateFields);

            return updatePlayer;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }
}
