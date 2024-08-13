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
                filter: Builders<NotificationModel>.Filter.Where(x => x.UserVisitingTeamModel.ClientId == clientId)) ?? Enumerable.Empty<NotificationModel>();

            if (notificationAll.Count() == 0)
            {
                throw new ExceptionFilter($"Não existe nenhuma notificação cadastrada");
            }

            return notificationAll;
        }
        catch (Exception ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<NotificationModel> GetIdNotificationAsync(string Id)
    {
        try
        {
            NotificationModel getId = await _notificationDao.GetIdAsync(Id);

            if (getId == null)
                throw new ExceptionFilter($"A notificação com o id '{Id}', não existe.");

            return getId;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<NotificationModel> SendNotificationAsync(NotificationModel createNotification)
    {
        try
        {
            // Validação do objeto de notificação
            if (createNotification == null)
            {
                throw new ExceptionFilter($"{nameof(createNotification)} O objeto de notificação não pode ser nulo.");
            }

            await _notificationDao.CreateAsync(createNotification);

            return createNotification;
        }catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<NotificationModel> ReadUpdateNotificationAsync(string Id, NotificationModel updateObject)
    {
        try
        {
            NotificationModel existingNotification = await GetIdNotificationAsync(Id);

            if(updateObject.ReadNotification == existingNotification.ReadNotification)
            {
                return existingNotification;
            }

            if (updateObject == null)
            {
                throw new ExceptionFilter($"{nameof(updateObject)} O objeto de notificação não pode ser nulo.");
            }

            Dictionary<string, object> updateFields = new()
            {
                { nameof(updateObject.ReadNotification), updateObject.ReadNotification ? updateObject.ReadNotification : existingNotification.ReadNotification },
            };

            NotificationModel notificationUpdate = await _notificationDao.UpdateAsync(Id, updateFields);

            return notificationUpdate;
        }
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }
}
