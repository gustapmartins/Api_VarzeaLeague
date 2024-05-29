using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Application.DTO.Team;

public class TeamViewDto
{
    public string Id { get; set; } = string.Empty;

    public string NameTeam { get; set; } = string.Empty;

    public string State { get; set; } = string.Empty;

    public string City { get; set; } = string.Empty;

    public string ClientId { get; set; } = string.Empty;

    public bool Active { get; set; }

    public DateTime DateCreated { get; set; }
}
