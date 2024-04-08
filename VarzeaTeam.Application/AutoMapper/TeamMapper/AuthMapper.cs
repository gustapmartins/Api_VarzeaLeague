using AutoMapper;
using VarzeaLeague.Application.DTO.User;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

public class AuthMapper : Profile
{
    public AuthMapper()
    {
        CreateMap<RegisterDto, UserModel>();
        CreateMap<LoginDto, UserModel>();
        CreateMap<UserViewDto, UserModel>();
        CreateMap<UserModel, UserViewDto>();
    }
}
