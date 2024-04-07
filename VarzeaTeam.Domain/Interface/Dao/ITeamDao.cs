using VarzeaLeague.Domain.Model;
using VarzeaTeam.Infra.Data.Repository.Utils;

namespace VarzeaLeague.Domain.Interface.Dao;

public interface ITeamDao : BaseDao<TeamModel>
{
    Task<TeamModel> TeamExist(string name);
}