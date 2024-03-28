
﻿using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaTeam.Domain.Model.Match;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Domain.Service;

public class MatchService : IMatchService
{
    private readonly IMatchDao _matchDao;

    public MatchService(IMatchDao matchDao)
    {
        _matchDao = matchDao;
    }

    public async Task<MatchModel> CreateAsync(MatchModel addObject)
    {
        try
        {
            await _matchDao.CreateAsync(addObject);

            return addObject;
        }
        catch (Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<List<MatchModel>> GetAsync()
    {
        try
        {
            List<MatchModel> GetAll = await _matchDao.GetAsync();

            if (GetAll.Count == 0)
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
            MatchModel findId = await _matchDao.GetIdAsync(Id);

            await _matchDao.UpdateAsync(Id, updateObject);

            return findId;
        }
        catch(Exception ex) 
        {
            throw new Exception(ex.Message);
        }
    }
}
