using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Domain.Model;

public class MatchModel : IEntity
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("HomeTeam")] // Atributo que mapeia essa propriedade para o campo 'homeTeam' no MongoDB
    public TeamModel HomeTeamModel { get; set; }

    [BsonElement("HomeTeamName")] // Atributo que mapeia essa propriedade para o campo 'HomeTeamName' no MongoDB
    public string HomeTeamName { get; set; } = string.Empty;

    [BsonElement("VisitingTeam")] // Atributo que mapeia essa propriedade para o campo 'visitingTeam' no MongoDB
    public TeamModel VisitingTeamModel { get; set; }

    [BsonElement("VisitingTeamName")] // Atributo que mapeia essa propriedade para o campo 'VisitingTeamName' no MongoDB
    public string VisitingTeamName { get; set; } = string.Empty;

    [BsonElement("Local")] // Atributo que mapeia essa propriedade para o campo 'local' no MongoDB
    public string Local { get; set; } = string.Empty;

    [BsonElement("Date")] // Atributo que mapeia essa propriedade para o campo 'date' no MongoDB
    public DateTime Date { get; set; }

    [BsonElement("TeamWin")] // Atributo que mapeia essa propriedade para o campo 'teamWin' no MongoDB
    public string TeamWin { get; set; } = string.Empty;

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
