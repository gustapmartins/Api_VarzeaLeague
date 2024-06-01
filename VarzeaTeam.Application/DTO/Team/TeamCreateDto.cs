using System.ComponentModel.DataAnnotations;

namespace VarzeaTeam.Application.DTO.Team;

public class TeamCreateDto
{
    [Required(ErrorMessage = "O Nome do time é obrigatório.")]
    public required string NameTeam { get; set; }

    [Required(ErrorMessage = "O Estado do time é obrigatório.")]
    public required string State { get; set; }

    [Required(ErrorMessage = "A Cidade do time é obrigatório.")]
    public required string City { get; set; }
}
