using VarzeaTeam.Domain.Model.Team;

namespace VarzeaLeague.Application.DTO.Match;

public class MatchViewDto
{
    public int Id { get; set; }

    public TeamModel HomeTeam { get; set; }

    public TeamModel VisitingTeam { get; set; }

    public string Local { get; set; }

    public DateTime Date { get; set; }

    public TeamModel TeamWin { get; set; }

    public bool Play { get; set; }
}
