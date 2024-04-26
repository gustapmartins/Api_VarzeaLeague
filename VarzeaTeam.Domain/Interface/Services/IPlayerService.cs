using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IPlayerService
{
    Task<IEnumerable<PlayerModel>> GetAsync(int page, int pageSize, string teamId);

    Task<PlayerModel> GetIdAsync(string Id);

    Task<PlayerModel> CreateAsync(PlayerModel addObject);

    Task<PlayerModel> RemoveAsync(string Id);

    Task<PlayerModel> UpdateAsync(string Id, PlayerModel updateObject);
}
