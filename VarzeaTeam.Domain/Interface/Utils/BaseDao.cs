﻿using MongoDB.Driver;

namespace VarzeaTeam.Infra.Data.Repository.Utils;

public interface BaseDao<T>
{
    Task<IEnumerable<T>> GetAsync(int page, int pageSize, FilterDefinition<T> filter);

    Task<T> GetIdAsync(string Id);

    Task CreateAsync(T addObject);

    Task RemoveAsync(string Id);

    Task<T> UpdateAsync(string Id, T updateObject);
}
