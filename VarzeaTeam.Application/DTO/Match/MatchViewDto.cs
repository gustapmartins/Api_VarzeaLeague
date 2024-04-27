namespace VarzeaLeague.Application.DTO.Match;

public class MatchViewDto
{
    public string Id { get; set; } = string.Empty;

    public string HomeTeamId { get; set; }

    public string VisitingTeamId { get; set; }

    public string Local { get; set; } = string.Empty;

    public DateTime Date { get; set; }

    public string TeamWin { get; set; }

    public DateTime DateCreated { get; set; }
}