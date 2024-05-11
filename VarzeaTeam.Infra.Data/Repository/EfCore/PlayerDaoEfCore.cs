using Microsoft.Extensions.Options;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Infra.Data.Context;
using MongoDB.Driver;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class PlayerDaoEfCore : BaseContext<PlayerModel>, IPlayerDao
{
    private readonly IMongoCollection<PlayerModel> _PlayerCollection;

    public PlayerDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "PlayerCollection")
    {
        _PlayerCollection = Collection;
    }

    public async Task<PlayerModel> PlayerExist(string name)
    {
        return await _PlayerCollection.Find(x => x.NamePlayer == name).FirstOrDefaultAsync<PlayerModel>();
    }
}
