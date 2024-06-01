using AutoMapper;
using VarzeaLeague.Application.DTO.Team;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.DTO.Team;

namespace VarzeaTeam.Application.AutoMapper.TeamMapper;

public class TeamMapper: Profile
{
    public TeamMapper() 
    {
        CreateMap<TeamCreateDto, TeamModel>();
        CreateMap<TeamViewDto, TeamModel>();
        CreateMap<TeamModel, TeamViewDto>();
        CreateMap<TeamUpdateDto, TeamModel>();
    }
}
