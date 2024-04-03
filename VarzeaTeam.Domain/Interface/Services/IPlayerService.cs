using VarzeaLeague.Domain.Interface.Utils;
using VarzeaTeam.Domain.Model.Player;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IPlayerService: BaseService<PlayerModel>
{
    Task ProduceAsync(string message);
}
