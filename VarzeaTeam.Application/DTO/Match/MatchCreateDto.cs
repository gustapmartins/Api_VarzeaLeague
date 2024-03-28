using System.ComponentModel.DataAnnotations;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Application.DTO.Match;

public class MatchCreateDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "O time da casa é obrigatório.")]
    public required TeamModel HomeTeam { get; set; }

    [Required(ErrorMessage = "O time visitante é obrigatório.")]
    public required TeamModel VisitingTeam { get; set; }

    public required string Local { get; set; }

    public DateTime Date { get; set; }

    public TeamModel TeamWin { get; set; }
}
