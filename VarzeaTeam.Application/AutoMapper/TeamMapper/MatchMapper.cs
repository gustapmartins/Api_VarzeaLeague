using AutoMapper;
using VarzeaLeague.Application.DTO.Match;
using VarzeaTeam.Application.DTO.Match;
using VarzeaTeam.Domain.Model.Match;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

public class MatchMapper: Profile
{
   public MatchMapper() 
   {
        CreateMap<MatchCreateDto, MatchModel>();
        CreateMap<MatchModel, MatchViewDto>();
        CreateMap<MatchModel, MatchUpdateDto>();
    }
}
