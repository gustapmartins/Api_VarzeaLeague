using AutoMapper;
using VarzeaLeague.Application.DTO.Match;
using VarzeaLeague.Application.DTO.Player;
using VarzeaTeam.Application.DTO.Match;
using VarzeaTeam.Domain.Model.Match;
using VarzeaTeam.Domain.Model.Player;

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
