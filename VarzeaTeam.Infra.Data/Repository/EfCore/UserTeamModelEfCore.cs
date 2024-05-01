using Microsoft.Extensions.Options;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Infra.Data.Context;
using MongoDB.Driver;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class UserTeamModelEfCore : BaseContext<UserTeamModel>, IUserTeamDao
{
    private readonly IMongoCollection<UserTeamModel> _UserTeamCollection;
    public UserTeamModelEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "UserTeamCollection")
    {
        _UserTeamCollection = Collection;
    }

    public async Task<UserTeamModel> UpdateAsync(string Id, UserTeamModel updateObject)
    {
        var filter = Builders<UserTeamModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<UserTeamModel>.Update.Combine();

        update = updateObject.TeamId != string.Empty ? update.Set(x => x.TeamId, updateObject.TeamId) : update;

        var options = new FindOneAndUpdateOptions<UserTeamModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _UserTeamCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
