using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaTeam.Domain.Model.Player;

public class PlayerModel
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDBw
    public string Id { get; set; }

    [BsonElement("name")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public string Name { get; set; }

    [BsonElement("Age")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public int Age { get; set; }
}
