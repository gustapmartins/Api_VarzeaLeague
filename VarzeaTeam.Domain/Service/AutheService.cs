using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Domain.Service;

public class AutheService : IAuthService
{

    public AutheService()
    {

    }

    public List<UserModel> FindAll()
    {
        throw new NotImplementedException();
    }

    public Task<string> ForgetPasswordAsync(string email)
    {
        throw new NotImplementedException();
    }

    public Task<string> Login(UserModel loginDto)
    {
        throw new NotImplementedException();
    }

    public Task<UserModel> RegisterAsync(UserModel registerDto)
    {
        throw new NotImplementedException();
    }

    public Task<string> ResetPasswordAsync(UserModel passwordResetDto)
    {
        throw new NotImplementedException();
    }
}
    