﻿using Microsoft.Extensions.Options;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model.DatabaseSettings;
using VarzeaLeague.Infra.Data.Context;
using MongoDB.Driver;
using System.Text.RegularExpressions;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Infra.Data.Repository.EfCore;

public class PlayerDaoEfCore : BaseContext<PlayerModel>, IPlayerDao
{
    private readonly IMongoCollection<PlayerModel> _PlayerCollection;

    public PlayerDaoEfCore(IOptions<VarzeaLeagueDatabaseSettings> options) : base(options, "PlayerCollection")
    {
        _PlayerCollection = Collection;
    }

    public async Task CreateAsync(PlayerModel addObject)
    {
        await _PlayerCollection.InsertOneAsync(addObject);
    }

    public async Task<List<PlayerModel>> GetAsync(int page, int pageSize)
    {
        int skip = (page - 1) * pageSize;

        var options = new FindOptions<PlayerModel>
        {
            Limit = pageSize,
            Skip = skip
        };

        return await _PlayerCollection.FindSync(_ => true, options).ToListAsync();
    }

    public async Task<PlayerModel> GetIdAsync(string Id)
    {
        return await _PlayerCollection.Find(x => x.Id == Id).FirstOrDefaultAsync<PlayerModel>();
    }

    public async Task<PlayerModel> PlayerExist(string name)
    {
        return await _PlayerCollection.Find(x => x.NamePlayer == name).FirstOrDefaultAsync<PlayerModel>();
    }

    public async Task RemoveAsync(string Id)
    {
        await _PlayerCollection.DeleteOneAsync(x => x.Id == Id);
    }

    public async Task<PlayerModel> UpdateAsync(string Id, PlayerModel updateObject)
    {
        var filter = Builders<PlayerModel>.Filter.Eq(x => x.Id, Id);
        var update = Builders<PlayerModel>.Update.Set(x => x.NamePlayer, updateObject.NamePlayer);

        var options = new FindOneAndUpdateOptions<PlayerModel>
        {
            ReturnDocument = ReturnDocument.After // Retorna o documento após a atualização
        };

        return await _PlayerCollection.FindOneAndUpdateAsync(filter, update, options);
    }
}
