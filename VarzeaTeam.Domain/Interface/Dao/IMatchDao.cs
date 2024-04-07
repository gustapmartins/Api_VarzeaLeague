using VarzeaLeague.Domain.Model;
using VarzeaTeam.Infra.Data.Repository.Utils;

namespace VarzeaLeague.Domain.Interface.Dao;

public interface IMatchDao : BaseDao<MatchModel>
{
    Task<MatchModel> MatchExistsAsync(string homeTeamId, string visitingTeamId);
}
