using AutoMapper;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Application.DTO.Notification;

namespace VarzeaLeague.Application.AutoMapper.TeamMapper;

public class NotificationMapper : Profile
{
    public NotificationMapper()
    {
        CreateMap<NotificationViewDto, NotificationModel>().ReverseMap();
    }
}
