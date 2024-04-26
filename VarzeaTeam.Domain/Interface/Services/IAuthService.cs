using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IAuthService
{
    Task<IEnumerable<UserModel>> GetAsync(int page, int pageSize);

    Task<UserModel> GetIdAsync(string Id);

    Task<UserModel> CreateAsync(UserModel addObject);

    Task<UserModel> RemoveAsync(string Id);

    Task<UserModel> UpdateAsync(string Id, UserModel updateObject); 

    Task<string> Login(UserModel userLogin);

    Task<string> ForgetPassword(string email);

    Task<string> ResetPassword(PasswordReset passwordReset);
}
