using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using VarzeaLeague.Application.DTO.Team;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.DTO.Team;

namespace VarzeaTeam.Application.AutoMapper.TeamMapper;

[ExcludeFromCodeCoverage]
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
