using AutoMapper;
using System.Diagnostics.CodeAnalysis;
using VarzeaLeague.Application.DTO.User;
using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

[ExcludeFromCodeCoverage]
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
