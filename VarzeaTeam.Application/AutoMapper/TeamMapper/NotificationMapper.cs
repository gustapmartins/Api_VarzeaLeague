using AutoMapper;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Application.DTO.Notification;
using System.Diagnostics.CodeAnalysis;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

[ExcludeFromCodeCoverage]
public class NotificationMapper : Profile
{
    public NotificationMapper()
    {
        CreateMap<NotificationViewDto, NotificationModel>().ReverseMap();
        CreateMap<NotificationUpdateDto, NotificationModel>().ReverseMap();
    }
}
