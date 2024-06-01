using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Application.DTO.Player;

public class PlayerViewDto
{
    public string Id { get; set; } = string.Empty;

    public string NamePlayer { get; set; } = string.Empty;

    public int Age { get; set; }

    public required TeamModel TeamModel { get; set; }
}
