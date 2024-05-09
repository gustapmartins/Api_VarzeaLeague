namespace VarzeaLeague.Application.DTO.Team;

public class TeamViewDto
{
    public string Id { get; set; } = string.Empty;

    public string NameTeam { get; set; } = string.Empty;

    public string clientId { get; set; } = string.Empty;

    public bool Active { get; set; }

    public DateTime DateCreated { get; set; }
}
