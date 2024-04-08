using System.ComponentModel.DataAnnotations;
using VarzeaTeam.Domain.Enum;

namespace VarzeaLeague.Application.DTO.User;

public class UserViewDto
{
    public string Id { get; set; } = string.Empty;

    public string Username { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    public string Cpf { get; set; } = string.Empty;

    public Role Role { get; set; } = Role.User;
}
