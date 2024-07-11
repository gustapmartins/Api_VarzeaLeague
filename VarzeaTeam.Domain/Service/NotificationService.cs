using Microsoft.AspNetCore.Http;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Domain.Exceptions;
using MongoDB.Driver;

namespace VarzeaLeague.Domain.Service;

public class NotificationService : INotificationService
{
    private readonly INotificationDao _notificationDao;
    private readonly HttpContext _httpContext;
    private readonly IGetClientIdToken _getClientIdFromToken;

    public NotificationService(INotificationDao notificationDao, IHttpContextAccessor httpContextAccessor, IGetClientIdToken getClientIdFromToken)
    {
        _notificationDao = notificationDao;
        _httpContext = httpContextAccessor.HttpContext!;
        _getClientIdFromToken = getClientIdFromToken;
    }

    public async Task<IEnumerable<NotificationModel>> GetNotificationAsync(int page, int pageSize)
    {
        try
        {
            string clientId = _getClientIdFromToken.GetClientIdFromToken(_httpContext);

            IEnumerable<NotificationModel> notificationAll = await _notificationDao.GetAsync(page, pageSize, 
                filter: Builders<NotificationModel>.Filter.Where(x => x.UserVisitingTeamModel.ClientId == clientId));

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
                throw new ExceptionFilter($"{nameof(notificationModel)} O objeto de notificação não pode ser nulo.");
            }

            await _notificationDao.CreateAsync(notificationModel);

            return notificationModel;
        }catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }
}
