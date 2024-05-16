using Microsoft.AspNetCore.Http;

namespace VarzeaLeague.Domain.Interface.Utils;

public interface IGetClientIdToken
{
    string GetClientIdFromToken(HttpContext context);
}
