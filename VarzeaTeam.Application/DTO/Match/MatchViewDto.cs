using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Application.DTO.Match;

public class MatchViewDto
{
    public string Id { get; set; } = string.Empty;

    public TeamModel HomeTeam { get; set; }

    public TeamModel VisitingTeam { get; set; }

    public string Local { get; set; }

    public DateTime Date { get; set; }

    public TeamModel TeamWin { get; set; }
}