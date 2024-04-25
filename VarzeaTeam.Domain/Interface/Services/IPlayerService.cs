using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IPlayerService : BaseService<PlayerModel>
{
    Task<IEnumerable<PlayerModel>> GetAsyncFilterPlayerTeam(int page, int pageSize, string teamId);
}
