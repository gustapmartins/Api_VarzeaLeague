using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaTeam.Application.DTO.Team;

public class TeamUpdateDto
{
    public string NameTeam { get; set; } = string.Empty;
}
