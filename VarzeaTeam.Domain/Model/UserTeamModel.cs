using MongoDB.Bson.Serialization.Attributes;
using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Domain.Model;

public class UserTeamModel : IEntity
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("User")] // Atributo que mapeia essa propriedade para o campo 'UserId' no MongoDB
    public UserModel UserModel { get; set; }

    [BsonElement("UserId")] // Atributo que mapeia essa propriedade para o campo 'UserId' no MongoDB
    public string UserId { get; set; } = string.Empty;

    [BsonElement("Team")] // Atributo que mapeia essa propriedade para o campo 'TeamId' no MongoDB
    public TeamModel TeamModel { get; set; }

    [BsonElement("TeamId")] // Atributo que mapeia essa propriedade para o campo 'TeamId' no MongoDB
    public string TeamId { get; set; } = string.Empty;

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'TeamId' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
