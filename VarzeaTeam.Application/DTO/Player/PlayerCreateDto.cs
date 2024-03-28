using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VarzeaTeam.Application.DTO.Player;

public class PlayerCreateDto
{
    [BsonId]
    [BsonRepresentation(MongoDB.Bson.BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonElement("name")]
    [Required(ErrorMessage = "O nome do jogador é obrigatório.")]
    public string Name { get; set; } = string.Empty;

    [BsonElement("age")]
    [Range(16, 50, ErrorMessage = "A idade do jogador deve estar entre 16 e 50 anos.")]
    public int Age { get; set; }

    [BsonElement("teamId")]
    [Required(ErrorMessage = "O ID do time é obrigatório.")]
    public string TeamId { get; set; } = string.Empty;
}
