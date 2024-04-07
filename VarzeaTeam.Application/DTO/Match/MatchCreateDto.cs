using System.ComponentModel.DataAnnotations;

namespace VarzeaTeam.Application.DTO.Match;

public class MatchCreateDto
{
    [Required(ErrorMessage = "O time da casa é obrigatório.")]
    public required string HomeTeamId { get; set; }

    [Required(ErrorMessage = "O time visitante é obrigatório.")]
    public required string VisitingTeamId { get; set; }

    [Required(ErrorMessage = "O Local é obrigatório.")]
    public required string Local { get; set; }

    [Required(ErrorMessage = "O dia é obrigatório.")]
    public DateTime Date { get; set; }
}
