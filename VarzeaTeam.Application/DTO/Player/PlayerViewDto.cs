using MongoDB.Bson.Serialization.Attributes;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaLeague.Application.DTO.Player;

public class PlayerViewDto
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("age")]
    public int Age { get; set; }

    [BsonElement("teamId")]
    public TeamModel Team { get; set; }
}
