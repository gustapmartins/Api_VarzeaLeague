using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using Microsoft.Extensions.Options;
using VarzeaLeague.Domain.Model;
using MongoDB.Driver;

namespace VarzeaTeam.Infra.Data.Repository.EfCore;

public class TeamDaoEfCore : BaseContext<TeamModel>, ITeamDao
{
    private readonly IMongoCollection<TeamModel> _TeamCollection;
    public TeamDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "TeamCollection")
    {
        _TeamCollection = Collection;
    }

    public async Task<TeamModel> TeamExist(string name)
    {
        return await _TeamCollection.Find(x => x.NameTeam.Equals(name)).FirstOrDefaultAsync<TeamModel>();
    }

    public async Task<TeamModel> UpdateAsync(string Id, TeamModel updateObject)
    {
        var filter = Builders<TeamModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<TeamModel>.Update.Combine();

        update = updateObject.NameTeam != string.Empty ? update.Set(x => x.NameTeam, updateObject.NameTeam) : update;

        update = updateObject.Active != false ? update.Set(x => x.Active, updateObject.Active) : update;

        var options = new FindOneAndUpdateOptions<TeamModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _TeamCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
