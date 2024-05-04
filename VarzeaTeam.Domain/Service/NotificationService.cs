using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.JwtHelper;
using VarzeaTeam.Domain.Exceptions;
using VarzeaLeague.Domain.Model;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;

namespace VarzeaLeague.Domain.Service;

public class NotificationService : INotificationService
{
    private readonly INotificationDao _notificationDao;
    private readonly HttpContext _httpContext;

    public NotificationService(INotificationDao notificationDao, IHttpContextAccessor httpContextAccessor)
    {
        _notificationDao = notificationDao;
        _httpContext = httpContextAccessor.HttpContext;
    }

    public async Task<IEnumerable<NotificationModel>> GetNotificationAsync(int page, int pageSize)
    {
        try
        {
            string clientId = GetTokenId.GetClientIdFromToken(_httpContext);

            IEnumerable<NotificationModel> notificationAll = await _notificationDao.GetAsync(page, pageSize, 
                filter: Builders<NotificationModel>.Filter.Where(x => x.UserVisitingId == clientId));

            if (notificationAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhuma notificação cadastrada");

            return notificationAll;
        }
        catch (Exception ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<NotificationModel> SendNotificationAsync(NotificationModel notificationModel)
    {
        try
        {
            // Validação do objeto de notificação
            if (notificationModel == null)
            {
                throw new ArgumentNullException(nameof(notificationModel), "O objeto de notificação não pode ser nulo.");
            }

            await _notificationDao.CreateAsync(notificationModel);

            return notificationModel;
        }catch(Exception ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }
}
