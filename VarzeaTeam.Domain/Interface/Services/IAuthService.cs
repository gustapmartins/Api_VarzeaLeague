using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IAuthService : BaseService<UserModel>
{
    Task<string> Login(UserModel userLogin);

    Task<string> ForgetPassword(string email);

    Task<string> ResetPassword(PasswordReset passwordReset);
}
