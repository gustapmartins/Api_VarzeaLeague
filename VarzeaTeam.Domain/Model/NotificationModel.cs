﻿using MongoDB.Bson.Serialization.Attributes;

namespace VarzeaLeague.Domain.Model;

public class NotificationModel
{
    [BsonId] // Atributo que indica que esta propriedade é o ID do documento no MongoDB
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)] // Representação do tipo de dados ObjectId do MongoDB
    public string Id { get; set; } = string.Empty;

    [BsonElement("UserHomeId")] // Atributo que mapeia essa propriedade para o campo 'homeTeam' no MongoDB
    public string UserHomeId { get; set; } = string.Empty;

    [BsonElement("UserVisitingId")] // Atributo que mapeia essa propriedade para o campo 'visitingTeam' no MongoDB
    public string UserVisitingId { get; set; } = string.Empty;

    [BsonElement("Mensage")] // Atributo que mapeia essa propriedade para o campo 'Mensage' no MongoDB
    public string Mensage {  get; set; } = string.Empty;

    [BsonElement("DateCreated")] // Atributo que mapeia essa propriedade para o campo 'DateCreated' no MongoDB
    public DateTime DateCreated { get; set; } = DateTime.Now;
}
