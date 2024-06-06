using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Application.DTO.Match;

public class MatchViewDto
{
    public string Id { get; set; } = string.Empty;

    public TeamModel HomeTeamModel { get; set; }

    public string HomeTeamName { get; set; } = string.Empty;

    public TeamModel VisitingTeamModel { get; set; }

    public string VisitingTeamName { get; set; } = string.Empty;

    public string Local { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string TeamWin { get; set; } = string.Empty;

    public DateTime DateCreated { get; set; }
}