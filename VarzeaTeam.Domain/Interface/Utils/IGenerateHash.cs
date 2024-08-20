using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Domain.Interface.Utils;

public interface IGenerateHash
{
    int GenerateRandomNumber();

    string GenerateHashRandom();

    string GenerateHashParameters(string password);

    bool VerifyPassword(string password, string hashedPassword);

    string GenerateToken(UserModel userModel);
}
