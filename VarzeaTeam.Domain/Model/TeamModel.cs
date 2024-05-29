using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Domain.Model;

public class TeamModel : IEntity
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("NameTeam")] // Atributo que mapeia essa propriedade para o campo 'nameTeam' no MongoDB
    public string NameTeam { get; set; } = string.Empty;

    [BsonElement("State")] // Atributo que mapeia essa propriedade para o campo 'nameTeam' no MongoDB
    public string State { get; set; } = string.Empty;

    [BsonElement("City")] // Atributo que mapeia essa propriedade para o campo 'nameTeam' no MongoDB
    public string City { get; set; } = string.Empty;

    [BsonElement("ClientId")] // Atributo que mapeia essa propriedade para o campo 'nameTeam' no MongoDB
    public string ClientId { get; set; } = string.Empty;

    [BsonElement("Active")] // Atributo que mapeia essa propriedade para o campo 'Active' no MongoDB
    public bool Active { get; set; } = true;

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
