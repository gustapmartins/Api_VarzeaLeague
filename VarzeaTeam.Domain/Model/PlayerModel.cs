using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VarzeaLeague.Domain.Model;

public class PlayerModel
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDBw
    public string Id { get; set; } = string.Empty;

    [BsonElement("namePlayer")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public string NamePlayer { get; set; } = string.Empty;

    [BsonElement("age")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public int Age { get; set; }

    [BsonElement("teamId")]
    public string TeamId { get; set; } = string.Empty;

    [BsonElement("team")]
    public TeamModel? Team { get; set; }

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
