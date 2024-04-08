using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IAuthService : BaseService<UserModel>
{
    Task<string> Login(UserModel userLogin);
}
