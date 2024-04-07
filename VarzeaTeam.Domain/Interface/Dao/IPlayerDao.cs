﻿using VarzeaLeague.Domain.Model;
using VarzeaTeam.Infra.Data.Repository.Utils;

namespace VarzeaLeague.Domain.Interface.Dao;

public interface IPlayerDao: BaseDao<PlayerModel>
{
    Task<PlayerModel> PlayerExist(string name);
}
