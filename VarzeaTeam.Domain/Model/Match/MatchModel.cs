using MongoDB.Bson.Serialization.Attributes;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Domain.Model.Match;

public class MatchModel
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("homeTeamId")] // Atributo que mapeia essa propriedade para o campo 'homeTeam' no MongoDB
    public string HomeTeamId { get; set; } = string.Empty;

    [BsonElement("homeTeam")] // Atributo que mapeia essa propriedade para o campo 'homeTeam' no MongoDB
    public TeamModel HomeTeam { get; set; }

    [BsonElement("visitingTeamId")] // Atributo que mapeia essa propriedade para o campo 'visitingTeam' no MongoDB
    public string VisitingTeamId { get; set; } = string.Empty;

    [BsonElement("visitingTeam")] // Atributo que mapeia essa propriedade para o campo 'visitingTeam' no MongoDB
    public TeamModel VisitingTeam { get; set; }

    [BsonElement("local")] // Atributo que mapeia essa propriedade para o campo 'local' no MongoDB
    public string Local { get; set; } = string.Empty;

    [BsonElement("date")] // Atributo que mapeia essa propriedade para o campo 'date' no MongoDB
    public DateTime Date { get; set; }

    [BsonElement("teamWin")] // Atributo que mapeia essa propriedade para o campo 'teamWin' no MongoDB
    public TeamModel TeamWin { get; set; }
}
