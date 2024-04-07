using AutoMapper;
using VarzeaLeague.Application.DTO.Match;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.DTO.Match;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

public class MatchMapper: Profile
{
   public MatchMapper() 
   {
        CreateMap<MatchCreateDto, MatchModel>();
        CreateMap<MatchViewDto, MatchModel>();
        CreateMap<MatchModel, MatchViewDto>();
        CreateMap<MatchModel, MatchUpdateDto>();
    }
}
