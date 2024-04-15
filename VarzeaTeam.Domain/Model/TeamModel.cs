using MongoDB.Bson.Serialization.Attributes;
using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Domain.Model;

public class TeamModel
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("NameTeam")] // Atributo que mapeia essa propriedade para o campo 'nameTeam' no MongoDB
    public string NameTeam { get; set; } = string.Empty;

    [BsonElement("UserId")] // Atributo que mapeia essa propriedade para o campo 'UserId' no MongoDB
    public string UserId { get; set; } = string.Empty;

    [BsonElement("UserModel")] // Atributo que mapeia essa propriedade para o campo 'UserModel' no MongoDB
    public UserModel? UserModel { get; set; }

    [BsonElement("Active")] // Atributo que mapeia essa propriedade para o campo 'Active' no MongoDB
    public bool Active { get; set; }
}
