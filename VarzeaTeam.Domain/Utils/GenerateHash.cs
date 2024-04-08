using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.Security.Cryptography;
using VarzeaLeague.Domain.Model;
using System.Security.Claims;
using System.Text;
using VarzeaTeam.Domain.Common;

namespace VarzeaLeague.Domain.Utils;

public static class GenerateHash
{
    public static string GenerateHashRandom()
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            string randomValue = Guid.NewGuid().ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(randomValue);
            byte[] hashBytes = sha256.ComputeHash(bytes);
            string hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash;
        }
    }


    public static string GenerateHashParameters(string password)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            // Converte a senha em bytes
            byte[] bytes = Encoding.UTF8.GetBytes(password);

            // Calcula o hash
            byte[] hashBytes = sha256.ComputeHash(bytes);

            // Converte o hash para uma string hexadecimal
            StringBuilder builder = new StringBuilder();
            foreach (byte b in hashBytes)
            {
                builder.Append(b.ToString("x2"));
            }
            return builder.ToString();
        }
    }

    public static bool VerifyPassword(string password, string hashedPassword)
    {
        // Gera o hash da senha fornecida pelo usuário
        string hashInputPassword = GenerateHashParameters(password);

        // Compara os hashes de forma segura, evitando ataques de tempo
        // Verifica se os comprimentos dos hashes são iguais antes de fazer a comparação
        // Isso evita ataques de tempo em que um invasor pode inferir a senha através do tempo de comparação
        return hashInputPassword == hashedPassword;
    }

    public static string GenerateToken(UserModel userModel)
    {
        try
        {
            Claim[] claims = new Claim[]
            {
                new Claim("username", userModel.UserName),
                new Claim("email", userModel.Email),
                new Claim("id", userModel.Id),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Constants.Key));

            var signingCredentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken
                (
                    expires: DateTime.Now.AddMinutes(10),
                    claims: claims,
                    signingCredentials: signingCredentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
