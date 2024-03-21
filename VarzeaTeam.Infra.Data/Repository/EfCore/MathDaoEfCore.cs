using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using VarzeaTeam.Domain.Model.Match;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class MathDaoEfCore : BaseContext<MatchModel>, IMatchDao
{
    private readonly IMongoCollection<MatchModel> _MatchCollection;

    public MathDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options)
    {
        _MatchCollection = Collection;
    }


    public async Task CreateAsync(MatchModel addObject)
    {
        await _MatchCollection.InsertOneAsync(addObject);
    }

    public async Task<List<MatchModel>> GetAsync()
    {
        return await _MatchCollection.Find(_ => true).ToListAsync();
    }

    public async Task<MatchModel> GetIdAsync(string Id)
    {
        return await _MatchCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(string Id)
    {
        await _MatchCollection.DeleteOneAsync(x => x.Id == Id);
    }

    public async Task UpdateAsync(string Id, MatchModel updateObject)
    {
        await _MatchCollection.ReplaceOneAsync(x => x.Id == Id, updateObject);
    }
}
