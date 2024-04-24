namespace VarzeaTeam.Application.DTO.Match;

public class MatchUpdateDto
{
    public string? HomeTeamId { get; set; }

    public string? VisitingTeamId { get; set; }

    public string? Local { get; set; }

    public DateTime? Date { get; set; }

    public string? TeamWinId { get; set; }
}
