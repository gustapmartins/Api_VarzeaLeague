using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaTeam.Domain.Model.Team;
using Microsoft.Extensions.Options;
using VarzeaLeague.Infra.Data.Context;
using VarzeaLeague.Domain.Interface.Dao;
using MongoDB.Driver;

namespace VarzeaTeam.Infra.Data.Repository.EfCore;

public class TeamDaoEfCore : BaseContext<TeamModel>, ITeamDao
{
    private readonly IMongoCollection<TeamModel> _TeamCollection;

    public TeamDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options)
    {
        _TeamCollection = Collection;
    }

    public async Task CreateAsync(TeamModel addObject)
    {
        await _TeamCollection.InsertOneAsync(addObject);
    }

    public async Task<List<TeamModel>> GetAsync()
    {
        return await _TeamCollection.Find(_ => true).ToListAsync();
    }

    public async Task<TeamModel> GetIdAsync(string Id)
    {
        return await _TeamCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(string Id)
    {
        await _TeamCollection.DeleteOneAsync(x => x.Id == Id);
    }

    public async Task UpdateAsync(string Id, TeamModel updateObject)
    {
        await _TeamCollection.ReplaceOneAsync(x => x.Id == Id, updateObject);
    }
}
