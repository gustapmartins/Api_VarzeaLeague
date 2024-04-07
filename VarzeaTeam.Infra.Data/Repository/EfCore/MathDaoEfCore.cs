using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Nest;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class MathDaoEfCore : BaseContext<MatchModel>, IMatchDao
{
    private readonly IMongoCollection<MatchModel> _MatchCollection;

    public MathDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "MatchCollection")
    {
        _MatchCollection = Collection;
    }

    public async Task CreateAsync(MatchModel addObject)
    {
        await _MatchCollection.InsertOneAsync(addObject);
    }

    public async Task<List<MatchModel>> GetAsync(int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;

        var options = new FindOptions<MatchModel>
        {
            Limit = pageSize,
            Skip = skip
        };

        return await _MatchCollection.FindSync(_ => true, options).ToListAsync();
    }

    public async Task<MatchModel> GetIdAsync(string Id)
    {
        return await _MatchCollection.Find(x => x.Id == Id).FirstOrDefaultAsync();
    }

    public async Task RemoveAsync(string Id)
    {
        await _MatchCollection.DeleteOneAsync(x => x.Id == Id);
    }

    public async Task<MatchModel> UpdateAsync(string Id, MatchModel updateObject)
    {
        var filter = Builders<MatchModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<MatchModel>.Update.Set(x => x.Local, updateObject.Local);

        var options = new FindOneAndUpdateOptions<MatchModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _MatchCollection.FindOneAndUpdateAsync(filter, update, options);
    }

    public async Task<MatchModel> MatchExistsAsync(string homeTeamId, string visitingTeamId)
    {
        MatchModel existingMatch = await _MatchCollection.Find(m => (m.HomeTeamId == homeTeamId && m.VisitingTeamId == visitingTeamId) ||
                                   (m.HomeTeamId == visitingTeamId && m.VisitingTeamId == homeTeamId)).FirstOrDefaultAsync();

        return existingMatch;
    }
}
