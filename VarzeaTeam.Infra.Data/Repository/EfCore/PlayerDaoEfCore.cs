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

    public async Task<PlayerModel> UpdateAsync(string Id, PlayerModel updateObject)
    {
        var filter = Builders<PlayerModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<PlayerModel>.Update.Combine();

        update = updateObject.NamePlayer != string.Empty ? update.Set(x => x.NamePlayer, updateObject.NamePlayer) : update;

        update = updateObject.Age != -1 ? update.Set(x => x.Age, updateObject.Age) : update;

        update = updateObject.TeamId != string.Empty ? update.Set(x => x.TeamId, updateObject.TeamId) : update;

        var options = new FindOneAndUpdateOptions<PlayerModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _PlayerCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
