using AutoMapper;
using VarzeaLeague.Application.DTO.User;
using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

public class AuthMapper : Profile
{
    public AuthMapper()
    {
        CreateMap<RegisterDto, UserModel>();
        CreateMap<LoginDto, UserModel>();
        CreateMap<UserViewDto, UserModel>();
        CreateMap<UserModel, UserViewDto>();
        CreateMap<PasswordResetDto, PasswordReset>().ReverseMap();
        CreateMap<UserUpdateDto, UserModel>().ReverseMap();
    }
}
