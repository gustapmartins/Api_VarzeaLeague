﻿using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Infra.Data.Context;
using Microsoft.Extensions.Options;
using VarzeaLeague.Domain.Model;
using MongoDB.Driver;

namespace VarzeaTeam.Infra.Data.Repository.EfCore;

public class TeamDaoEfCore : BaseContext<TeamModel>, ITeamDao
{
    private readonly IMongoCollection<TeamModel> _TeamCollection;
    public TeamDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "TeamCollection")
    {
        _TeamCollection = Collection;
    }

    public async Task CreateAsync(TeamModel addObject)
    {   
        await _TeamCollection.InsertOneAsync(addObject);
    }

    public async Task<IEnumerable<TeamModel>> GetAsync(int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;

        FilterDefinition<TeamModel> filter = Builders<TeamModel>.Filter.Where(x => x.Active == true);

        FindOptions<TeamModel> options = new()
        {
            Limit = pageSize,
            Skip = skip,
            Sort = Builders<TeamModel>.Sort.Descending(x => x.DateCreated) // Ordena por data de criação no próprio banco de dados
        };

        return await _TeamCollection.FindSync(filter, options).ToListAsync();
    }

    public async Task<TeamModel> GetIdAsync(string Id)
    {
        return await _TeamCollection.Find(x => x.Id == Id).FirstOrDefaultAsync<TeamModel>();
    }

    public async Task RemoveAsync(string Id)
    {
        await _TeamCollection.DeleteOneAsync(x => x.Id == Id);
    }

    public async Task<TeamModel> TeamExist(string name)
    {
        return await _TeamCollection.Find(x => x.NameTeam.Equals(name)).FirstOrDefaultAsync<TeamModel>();
    }

    public async Task<TeamModel> UpdateAsync(string Id, TeamModel updateObject)
    {
        var filter = Builders<TeamModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<TeamModel>.Update.Combine();
            
        update = updateObject.NameTeam != null ? update.Set(x => x.NameTeam, updateObject.NameTeam) : update;

        var options = new FindOneAndUpdateOptions<TeamModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _TeamCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
