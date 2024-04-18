using System.ComponentModel.DataAnnotations;

namespace VarzeaLeague.Application.DTO.User;

public class PasswordResetDto
{
    [Required(ErrorMessage = "O Password is required")]
    public string? Password { get; set; }

    [Required(ErrorMessage = "Confirm your password")]
    [Compare("Password")]
    public string? ConfirmPassword { get; set; }

    [Required(ErrorMessage = "token is required")]
    public string? Token { get; set; }
}
