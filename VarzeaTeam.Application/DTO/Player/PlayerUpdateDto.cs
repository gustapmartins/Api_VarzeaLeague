using MongoDB.Bson.Serialization.Attributes;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Application.DTO.Player;

public class PlayerUpdateDto
{
    public string NamePlayer { get; set; } = string.Empty;

    public int Age { get; set; }

    public string TeamId { get; set; } = string.Empty;
}
