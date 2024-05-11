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
}
