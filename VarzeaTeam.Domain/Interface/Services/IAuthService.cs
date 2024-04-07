using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Interface.Services;

public interface IAuthService
{
    List<UserModel> FindAll();

    Task<string> Login(UserModel loginDto);

    Task<UserModel> RegisterAsync(UserModel registerDto);

    Task<string> ForgetPasswordAsync(string email);

    Task<string> ResetPasswordAsync(UserModel passwordResetDto);
}
