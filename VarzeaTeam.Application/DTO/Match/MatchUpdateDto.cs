namespace VarzeaTeam.Application.DTO.Match;

public class MatchUpdateDto
{
    public string? HomeTeamName { get; set; }

    public string? VisitingTeamName { get; set; }

    public string? Local { get; set; }

    public DateTime? Date { get; set; }

    public string? TeamWinId { get; set; }
}
