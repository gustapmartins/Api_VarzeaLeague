using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VarzeaTeam.Application.DTO.Team;

public class TeamCreateDto
{
    [Required(ErrorMessage = "O nome do time é obrigatório.")]
    public required string NameTeam { get; set; }

    [Required(ErrorMessage = "O nome do estado é obrigatório.")]
    public required string State { get; set; }

    [Required(ErrorMessage = "O nome do cidade é obrigatório.")]
    public required string City { get; set; }
}
