using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using VarzeaLeague.Domain.Model.User;
using VarzeaLeague.Domain.Utils;
using VarzeaLeague.Domain.Enum;
using Elasticsearch.Net;
using Nest;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class AuthDaoEfCore : BaseContext<UserModel>, IAuthDao
{
    private readonly IMongoCollection<UserModel> _AuthCollection;

    public AuthDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "AuthCollection")
    {
        _AuthCollection = Collection;
    }

    public async Task<IEnumerable<UserModel>> GetAsync(int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;

        FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Where(x => x.AccountStatus == AccountStatus.active); // Supondo que 1 represente o status de conta ativa

        FindOptions<UserModel> options = new()
        {
            Limit = pageSize,
            Skip = skip,
            Sort = Builders<UserModel>.Sort.Descending(x => x.DateCreated) // Ordena os usuários por data de criação no próprio banco de dados
        };

        return await _AuthCollection.FindSync(filter, options).ToListAsync();
    }

    public async Task<UserModel> GetIdAsync(string Id)
    {
        return await _AuthCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
    }

    public async Task<UserModel> FindEmail(string Email)
    {
        return await _AuthCollection.Find(x => x.Email == Email).FirstOrDefaultAsync();
    }

    public async Task CreateAsync(UserModel addObject)
    {
        await _AuthCollection.InsertOneAsync(addObject);
    }

    public async Task RemoveAsync(string Id)
    {
        await _AuthCollection.DeleteOneAsync(x => x.Id == Id);
    }

    public async Task<UserModel> UpdateAsync(string Id, UserModel updateObject)
    {
        var filter = Builders<UserModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<UserModel>.Update.Combine();

        if (!string.IsNullOrEmpty(updateObject.UserName))
        {
            update = update.Set(x => x.UserName, updateObject.UserName);
        }

        if (!string.IsNullOrEmpty(updateObject.Password))
        {
            update = update.Set(x => x.Password, GenerateHash.GenerateHashParameters(updateObject.Password));
        }

        if (updateObject.AccountStatus != default(AccountStatus))
        {
            update = update.Set(x => x.AccountStatus, updateObject.AccountStatus);
        }

        var options = new FindOneAndUpdateOptions<UserModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _AuthCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
