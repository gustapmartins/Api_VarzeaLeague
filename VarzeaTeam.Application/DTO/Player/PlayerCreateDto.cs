using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace VarzeaTeam.Application.DTO.Player;

public class PlayerCreateDto
{
    [Required(ErrorMessage = "O nome do jogador é obrigatório.")]
    public string NamePlayer { get; set; } = string.Empty;

    [Range(16, 50, ErrorMessage = "A idade do jogador deve estar entre 16 e 50 anos.")]
    public int Age { get; set; }

    [Required(ErrorMessage = "O ID do time é obrigatório.")]
    public string TeamId { get; set; } = string.Empty;
}
