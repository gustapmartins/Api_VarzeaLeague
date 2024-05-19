using VarzeaLeague.Domain.Model.User;
using VarzeaTeam.Infra.Data.Repository.Utils;

namespace VarzeaLeague.Domain.Interface.Dao;

public interface IAuthDao : BaseDao<UserModel>
{
    Task<UserModel> FindEmail(string Email);
}
