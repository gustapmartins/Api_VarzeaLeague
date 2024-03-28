using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VarzeaTeam.Application.DTO.Team;

public class TeamCreateDto
{
    [BsonElement("name")] // Atributo que mapeia essa propriedade para o campo 'name' no MongoDB
    [Required(ErrorMessage = "O nome do time é obrigatório.")]
    public required string Name { get; set; }
}
