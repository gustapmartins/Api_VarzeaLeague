using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaTeam.Domain.Model.Match;

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
        await _matchDao.CreateAsync(addObject);

        return addObject;
    }

    public async Task<List<MatchModel>> GetAsync()
    {
        return await _matchDao.GetAsync();
    }

    public async Task<MatchModel> GetIdAsync(string Id)
    {
        return await _matchDao.GetIdAsync(Id);
    }

    public async Task<MatchModel> RemoveAsync(string Id)
    {
        var findId = await _matchDao.GetIdAsync(Id);

        await _matchDao.RemoveAsync(Id);

        return findId;
    }

    public async Task<MatchModel> UpdateAsync(string Id, MatchModel updateObject)
    {
        var findId = await _matchDao.GetIdAsync(Id);

        await _matchDao.UpdateAsync(Id, updateObject);

        return findId;
    }
}
