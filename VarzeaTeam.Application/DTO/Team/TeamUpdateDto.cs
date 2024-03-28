using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaTeam.Application.DTO.Team;

public class TeamUpdateDto
{
    [BsonElement("name")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public string Name { get; set; } = string.Empty;
}
