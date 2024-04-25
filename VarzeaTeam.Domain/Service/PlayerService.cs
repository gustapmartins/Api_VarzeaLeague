using System.Collections.Immutable;
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

    public async Task<IEnumerable<PlayerModel>> GetAsyncFilterPlayerTeam(int page, int pageSize, string teamId)
    {
        try
        {
            IEnumerable<PlayerModel> playerAll = await _playerDao.GetAsync(page, pageSize);

            if (playerAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhum time cadastrado");

            TeamModel teamModel = await _teamService.GetIdAsync(teamId);

            IEnumerable<PlayerModel> filterPlayers = playerAll.Where(x => x.TeamId == teamModel.Id).ToImmutableList();

            return filterPlayers;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }


    //Possiveis solucoes de melhoria 
    //usar esse metodo no metodo de cima e usar uma rota só para fazer a consulta dos jogadores, e esse filtro se aplica para a rota principal
    public async Task<IEnumerable<PlayerModel>> GetAsync(int page, int pageSize)
    {
        try
        {
            IEnumerable<PlayerModel> playerAll = await _playerDao.GetAsync(page, pageSize);

            if (playerAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhum time cadastrado");

            return playerAll;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PlayerModel> GetIdAsync(string Id)
    {
        try
        {
            PlayerModel player = await _playerDao.GetIdAsync(Id);

            if (player == null)
                throw new ExceptionFilter($"O jogador com o id '{Id}' não existe.");

            // Buscar o objeto de time associado a este jogador
            TeamModel team = await _teamService.GetIdAsync(player.TeamId);

            return player;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PlayerModel> CreateAsync(PlayerModel addObject)
    {
        try
        {
            PlayerModel existingPlayer = await _playerDao.PlayerExist(addObject.NamePlayer);

            if(existingPlayer != null)
                throw new Exception($"O jogador com o nome '{addObject.NamePlayer}' já existe.");

            TeamModel team = await _teamService.GetIdAsync(addObject.TeamId);

            addObject.TeamId = team.Id;

            await _playerDao.CreateAsync(addObject);

            return addObject;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PlayerModel> RemoveAsync(string Id)
    {
        try
        {
            PlayerModel findId = await _playerDao.GetIdAsync(Id);

            await _playerDao.RemoveAsync(Id);

            return findId;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<PlayerModel> UpdateAsync(string Id, PlayerModel updateObject)
    {
        try
        {
            PlayerModel findId = await _playerDao.GetIdAsync(Id);

            PlayerModel updatePlayer = await _playerDao.UpdateAsync(Id, updateObject);

            return updatePlayer;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }
}
