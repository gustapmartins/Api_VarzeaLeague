using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class MathDaoEfCore : BaseContext<MatchModel>, IMatchDao
{
    private readonly IMongoCollection<MatchModel> _MatchCollection;

    public MathDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "MatchCollection")
    {
        _MatchCollection = Collection;
    }

    public async Task<MatchModel> MatchExistsAsync(string homeTeamId, string visitingTeamId)
    {
        MatchModel existingMatch = await _MatchCollection.Find(m => (m.HomeTeamId == homeTeamId && m.VisitingTeamId == visitingTeamId) ||
                                   (m.HomeTeamId == visitingTeamId && m.VisitingTeamId == homeTeamId)).FirstOrDefaultAsync();

        return existingMatch;
    }
}
