using AutoMapper;
using VarzeaLeague.Application.DTO.Player;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.DTO.Player;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

public class PlayerMapper : Profile
{
    public PlayerMapper()
    {
        CreateMap<PlayerCreateDto, PlayerModel>();
        CreateMap<PlayerViewDto, PlayerModel>();
        CreateMap<PlayerModel, PlayerViewDto>();
        CreateMap<PlayerUpdateDto, PlayerModel>();
    }
}
