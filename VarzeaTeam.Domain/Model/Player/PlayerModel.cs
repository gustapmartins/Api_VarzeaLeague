using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Domain.Model.Player;

public class PlayerModel
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDBw
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public string Name { get; set; } = string.Empty;

    [BsonElement("Age")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public int Age { get; set; }

    [BsonElement("teamId")]
    public string TeamId { get; set; } = string.Empty;
}
