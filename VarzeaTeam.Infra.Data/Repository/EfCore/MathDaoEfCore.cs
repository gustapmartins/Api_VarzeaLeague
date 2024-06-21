using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using Microsoft.Extensions.Options;
using VarzeaLeague.Domain.Model;
using MongoDB.Driver;

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
        MatchModel existingMatch = await _MatchCollection.Find(m => m.TeamWin == string.Empty && (m.HomeTeamModel.Id == homeTeamId && m.VisitingTeamModel.Id == visitingTeamId) ||
                                   m.TeamWin == string.Empty && (m.HomeTeamModel.Id == visitingTeamId && m.VisitingTeamModel.Id == homeTeamId)).FirstOrDefaultAsync();

        return existingMatch;
    }
}
