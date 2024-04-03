﻿using Confluent.Kafka;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Domain.Exceptions;
using VarzeaTeam.Domain.Model.Player;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaLeague.Domain.Service;

public class PlayerService : IPlayerService
{
    private readonly IPlayerDao _playerDao;
    private readonly ITeamDao _teamDao;

    public PlayerService(IPlayerDao playerDao, ITeamDao teamDao)
    {
        _playerDao = playerDao;
        _teamDao = teamDao;
    }

    public async Task<List<PlayerModel>> GetAsync()
    {
        try
        {
            List<PlayerModel> GetAll = await _playerDao.GetAsync();

            if (GetAll.Count == 0)
                throw new ExceptionFilter($"Não existe nenhum time cadastrado");

            return GetAll;
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
            TeamModel team = await _teamDao.GetIdAsync(player.TeamId);

            if (team == null)
                throw new Exception($"O time com o ID '{player.TeamId}' não foi encontrado.");

           
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

            TeamModel team = await _teamDao.GetIdAsync(addObject.TeamId);

            if(team == null)
                throw new Exception($"O time com o ID '{addObject.TeamId}' não foi encontrado.");

            addObject.TeamId = team.Id;
            addObject.Team = team;

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

            await _playerDao.UpdateAsync(Id, updateObject);

            return findId;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task ProduceAsync(string message)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092"
        };

        var producer = new ProducerBuilder<Null, string>(config).Build();

        try
        {
            var deliveryResult = await producer.ProduceAsync("varzea-league", new Message<Null, string> { Value = message });

            Console.WriteLine($"Mensagem produzida: '{message}', Offset: {deliveryResult.Offset}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao produzir mensagem: {ex.Message}");
        }
    }
}
