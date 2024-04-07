using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using VarzeaTeam.Domain.Model.Team;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VarzeaTeam.Domain.Model.Player;

namespace VarzeaTeam.Infra.Data.Repository.EfCore;

public class TeamDaoEfCore : BaseContext<TeamModel>, ITeamDao
{
    private readonly IMongoCollection<TeamModel> _TeamCollection;
    public TeamDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "TeamCollection")
    {
        _TeamCollection = Collection;
    }

    public async Task CreateAsync(TeamModel addObject)
    {   
        await _TeamCollection.InsertOneAsync(addObject);
    }

    public async Task<List<TeamModel>> GetAsync(int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;

        var options = new FindOptions<TeamModel>
        {
            Limit = pageSize,
            Skip = skip
        };

        return await _TeamCollection.FindSync(_ => true, options).ToListAsync();
    }

    public async Task<TeamModel> GetIdAsync(string Id)
    {
        return await _TeamCollection.Find(x => x.Id == Id).FirstOrDefaultAsync<TeamModel>();
    }

    public async Task RemoveAsync(string Id)
    {
        await _TeamCollection.DeleteOneAsync(x => x.Id == Id);
    }

    public async Task<TeamModel> TeamExist(string name)
    {
        return await _TeamCollection.Find(x => x.NameTeam.Equals(name)).FirstOrDefaultAsync<TeamModel>();
    }

    public async Task<TeamModel> UpdateAsync(string Id, TeamModel updateObject)
    {
        var filter = Builders<TeamModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<TeamModel>.Update.Set(x => x.NameTeam, updateObject.NameTeam);

        var options = new FindOneAndUpdateOptions<TeamModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _TeamCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
