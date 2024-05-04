﻿using Microsoft.AspNetCore.Http;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.JwtHelper;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Domain.Service;

public class NotificationService : INotificationService
{
    private readonly INotificationDao _notificationDao;
    private readonly HttpContext _httpContext;

    public NotificationService(INotificationDao notificationDao, HttpContext httpContext)
    {
        _notificationDao = notificationDao;
        _httpContext = httpContext;
    }
    public async Task<IEnumerable<NotificationModel>> GetNotificationAsync()
    {
        try
        {
            IEnumerable<NotificationModel> notificationAll = await _notificationDao.GetAsync();

            if (notificationAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhuma notificação cadastrada");

            return notificationAll;
        }
        catch (Exception ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<NotificationModel> SendNotificationAsync(NotificationModel NotificationModel)
    {
        try
        {
            NotificationModel notificationModel = new()
            {
                
            };

            await _notificationDao.CreateAsync(NotificationModel);

            return NotificationModel;
        }catch(Exception ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<NotificationModel> DeleteNotificationAsync(string idNotification)
    {
        try
        {
            NotificationModel findId = await _notificationDao.GetIdAsync(idNotification);

            if(findId == null)
                throw new ExceptionFilter($"O id '{idNotification}' não existe."); 

            await _notificationDao.RemoveAsync(idNotification);

            return findId;
        }
        catch (Exception ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }
}
