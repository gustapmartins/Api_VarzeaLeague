using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Application.DTO.Team;

public class TeamViewDto
{
    [BsonElement("name")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public string Name { get; set; } = string.Empty;
}
