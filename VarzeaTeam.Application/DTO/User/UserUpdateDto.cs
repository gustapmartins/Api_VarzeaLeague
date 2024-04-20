using VarzeaLeague.Domain.Enum;

namespace VarzeaLeague.Application.DTO.User;

public class UserUpdateDto
{
    public string Username { get; set; } = string.Empty;

    public AccountStatus AccountStatus { get; set; }
}
