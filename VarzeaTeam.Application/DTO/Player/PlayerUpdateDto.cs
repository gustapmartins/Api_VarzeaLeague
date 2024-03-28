﻿using MongoDB.Bson.Serialization.Attributes;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Application.DTO.Player;

public class PlayerUpdateDto
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("age")]
    public int Age { get; set; }

    [BsonElement("teamId")]
    public string TeamId { get; set; } = string.Empty;
}
