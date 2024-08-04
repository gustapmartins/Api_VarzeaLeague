using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Domain.Model;

public class NotificationModel : IEntity
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("UserHomeTeamModel")] // Atributo que mapeia essa propriedade para o campo 'visitingTeam' no MongoDB
    public TeamModel? UserHomeTeamModel { get; set; } = new TeamModel();

    [BsonElement("UserVisitingTeamModel")] // Atributo que mapeia essa propriedade para o campo 'visitingTeam' no MongoDB
    public TeamModel? UserVisitingTeamModel { get; set; } = new TeamModel();

    [BsonElement("NotificationType")]
    public string NotificationType { get; set; } = string.Empty;

    [BsonElement("ReadNotification")]
    public bool ReadNotification { get; set; }

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
