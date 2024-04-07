using System.Security.Cryptography;
using System.Text;

namespace VarzeaLeague.Domain.Interface.Utils;

public static class GenerateHash
{
    public static string GenerateHashUT8()
    {
        string randomValue = Guid.NewGuid().ToString();
        SHA256 sha256 = SHA256.Create();
        byte[] bytes = Encoding.UTF8.GetBytes(randomValue);
        byte[] hashBytes = sha256.ComputeHash(bytes);
        string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        return hash;
    }
}
