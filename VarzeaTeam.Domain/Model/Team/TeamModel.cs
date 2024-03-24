using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaTeam.Domain.Model.Team;

public interface IEntity
{
    string Id { get; set; }
}

public class TeamModel: IEntity
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; }

    [BsonElement("name")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    public string Name { get; set; }
}
