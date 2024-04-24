using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Domain.Model;

public class MatchModel
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("homeTeamId")] // Atributo que mapeia essa propriedade para o campo 'homeTeam' no MongoDB
    public string HomeTeamId { get; set; } = string.Empty;

    [BsonElement("visitingTeamId")] // Atributo que mapeia essa propriedade para o campo 'visitingTeam' no MongoDB
    public string VisitingTeamId { get; set; } = string.Empty;

    [BsonElement("local")] // Atributo que mapeia essa propriedade para o campo 'local' no MongoDB
    public string Local { get; set; } = string.Empty;

    [BsonElement("date")] // Atributo que mapeia essa propriedade para o campo 'date' no MongoDB
    public DateTime Date { get; set; }

    [BsonElement("teamWin")] // Atributo que mapeia essa propriedade para o campo 'teamWin' no MongoDB
    public string? TeamWin { get; set; }

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
