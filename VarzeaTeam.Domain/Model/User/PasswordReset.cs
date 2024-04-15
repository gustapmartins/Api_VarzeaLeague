namespace VarzeaLeague.Domain.Model.User;

public class PasswordReset
{
    public string Token { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}