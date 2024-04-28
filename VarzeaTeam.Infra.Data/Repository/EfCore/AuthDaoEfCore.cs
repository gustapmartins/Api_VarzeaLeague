using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Model.User;
using VarzeaLeague.Domain.Utils;
using VarzeaLeague.Infra.Data.Context;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class AuthDaoEfCore : BaseContext<UserModel>, IAuthDao
{
    private readonly IMongoCollection<UserModel> _AuthCollection;

    public AuthDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "AuthCollection")
    {
        _AuthCollection = Collection;
    }

    public async Task<UserModel> FindEmail(string Email)
    {
        return await _AuthCollection.Find(x => x.Email == Email).FirstOrDefaultAsync();
    }

    public async Task<UserModel> UpdateAsync(string Id, UserModel updateObject)
    {
        var filter = Builders<UserModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<UserModel>.Update.Combine();

        update = updateObject.UserName != null ? update.Set(x => x.UserName, updateObject.UserName) : update;
        
        update = updateObject.Password != null ? update.Set(x => x.Password, GenerateHash.GenerateHashParameters(updateObject.Password)) : update;
        
        update = updateObject.AccountStatus != null ? update.Set(x => x.AccountStatus, updateObject.AccountStatus) : update;

        var options = new FindOneAndUpdateOptions<UserModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _AuthCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
