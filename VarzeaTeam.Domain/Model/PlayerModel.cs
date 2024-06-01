using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Domain.Model;

public class PlayerModel : IEntity
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDBw
    public string Id { get; set; } = string.Empty;

    [BsonElement("NamePlayer")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public string NamePlayer { get; set; } = string.Empty;

    [BsonElement("Age")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public int Age { get; set; }

    [BsonElement("Team")]
    public TeamModel TeamModel { get; set; }

    [BsonElement("TeamId")]
    public string TeamId { get; set; } = string.Empty;

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
