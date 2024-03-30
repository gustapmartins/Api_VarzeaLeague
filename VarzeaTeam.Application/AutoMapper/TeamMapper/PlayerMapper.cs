using AutoMapper;
using VarzeaLeague.Application.DTO.Player;
using VarzeaTeam.Application.DTO.Player;
using VarzeaTeam.Domain.Model.Player;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

public class PlayerMapper : Profile
{
    public PlayerMapper()
    {
        CreateMap<PlayerCreateDto, PlayerModel>();
        CreateMap<PlayerViewDto, PlayerModel>();
        CreateMap<PlayerModel, PlayerViewDto>();
        CreateMap<PlayerModel, PlayerUpdateDto>();
    }
}
