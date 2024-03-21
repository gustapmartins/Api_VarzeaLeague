using AutoMapper;
using VarzeaLeague.Application.DTO.Team;
using VarzeaTeam.Application.DTO.Team;
using VarzeaTeam.Domain.Model.Team;

namespace VarzeaTeam.Application.AutoMapper.TeamMapper;

public class TeamMapper: Profile
{
    public TeamMapper() 
    {
        CreateMap<TeamCreateDto, TeamModel>();
        CreateMap<TeamModel, TeamViewDto>();
        CreateMap<TeamModel, TeamUpdateDto>();
    }
}
