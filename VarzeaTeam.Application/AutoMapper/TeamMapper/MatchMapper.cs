using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using VarzeaLeague.Application.DTO.Match;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.DTO.Match;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

[ExcludeFromCodeCoverage]
public class MatchMapper: Profile
{
   public MatchMapper() 
   {
        CreateMap<MatchCreateDto, MatchModel>().ReverseMap();
        CreateMap<MatchModel, MatchViewDto>().ReverseMap();
        CreateMap<MatchModel, MatchUpdateDto>().ReverseMap();
        CreateMap<MatchUpdateDto, MatchModel>();
    }
}
