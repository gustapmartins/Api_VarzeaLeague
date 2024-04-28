using MongoDB.Driver;
using System.Collections.Immutable;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Domain.Service;

public class PlayerService : IPlayerService
{
    private readonly IPlayerDao _playerDao;
    private readonly ITeamDao _teamModel;

    public PlayerService(IPlayerDao playerDao, ITeamDao teamModel)
    {
        _playerDao = playerDao;
        _teamModel = teamModel;
    }

    public async Task<IEnumerable<PlayerModel>> GetAsync(int page, int pageSize, string teamId)
    {
        try
        {
            TeamModel teamModel = await _teamModel.GetIdAsync(teamId);

            IEnumerable<PlayerModel> playerAll = await _playerDao.GetAsync(page, pageSize, 
                filter: teamModel != null ? Builders<PlayerModel>.Filter.Where(x => x.TeamId == teamModel.Id) : null);

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
            TeamModel team = await _teamModel.GetIdAsync(player.TeamId);

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

            TeamModel team = await _teamModel.GetIdAsync(addObject.TeamId);

            addObject.TeamId = team.Id;
            addObject.TeamModel = team;

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
