﻿using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Model.User;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Domain.Service;

public class AuthService : IAuthService
{
    private readonly IAuthDao _authDao;
    private readonly IGetClientIdToken _getClientIdToken;
    private readonly HttpContext _httpContext;
    private readonly IEmailService _emailService;
    private readonly IMemoryCacheService _memoryCacheService;
    private readonly IGenerateHash _generateHash;

    public AuthService(
        IAuthDao authDao,
        IEmailService emailService,
        IMemoryCacheService memoryCacheService,
        IGenerateHash generateHash,
        IGetClientIdToken getClientIdToken,
        IHttpContextAccessor httpContextAccessor)
    {
        _authDao = authDao;
        _emailService = emailService;
        _generateHash = generateHash;
        _memoryCacheService = memoryCacheService;
        _getClientIdToken = getClientIdToken;
        _httpContext = httpContextAccessor.HttpContext!;
    }

    public async Task<IEnumerable<UserModel>> GetAsync(int page, int pageSize)
    {
        try
        {
            IEnumerable<UserModel> GetAll = await _authDao.GetAsync(page, pageSize, filter: Builders<UserModel>.Filter.Where(x => x.AccountStatus == AccountStatus.active));

            if (GetAll.Count() == 0)
            {
                throw new ExceptionFilter($"Não existe nenhum usuário cadastrada");
            }

            return GetAll;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<UserModel> GetIdAsync(string Id)
    {
        try
        {
            UserModel GetId = await _authDao.GetIdAsync(Id);

            if (GetId == null)
            {
                throw new ExceptionFilter($"O usuário com o id '{Id}', não existe.");
            }

            if (GetId.AccountStatus == 0)
            {
                throw new ExceptionFilter($"Esse usuário '{Id}' está bloqueado.");
            }

            return GetId;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<string> Login(UserModel userLogin)
    {
        try
        {
            UserModel findUser = await _authDao.FindEmail(userLogin.Email);

            if (findUser == null)
            {
                throw new ExceptionFilter($"Este email: {userLogin.Email} não existe.");
            }

            //Faz uma validação para verificar se a senha que o usuariop está passando corresponde a senha salva no banco, em formato hash
            bool isPasswordCorrect = _generateHash.VerifyPassword(userLogin.Password, findUser.Password);

            if (!isPasswordCorrect)
            {
                throw new UnauthorizedAccessException("Senha incorreta");
            }

            //Gera um token a partir do usuario buscado pelo E-mail
            string token = _generateHash.GenerateToken(findUser);

            return token;
        }
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message);
        }
    }

    public async Task<UserModel> CreateAsync(UserModel addObject)
    {
        try
        {
            UserModel findEmail = await _authDao.FindEmail(addObject.Email);

            if (findEmail != null)
            {
                throw new ExceptionFilter($"usuário com esse email: '{addObject.Email}', já existe.");
            }

            addObject.Password = _generateHash.GenerateHashParameters(addObject.Password);
            addObject.AccountStatus = AccountStatus.active;
            addObject.DateCreated = DateTime.UtcNow;

            await _authDao.CreateAsync(addObject);

            return addObject;
        }
        catch(ExceptionFilter ex)
        {
           throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<string> ForgetPassword(string email)
    {
        try
        {
            UserModel findEmail = await _authDao.FindEmail(email);

            if (findEmail == null)
            {
                throw new ExceptionFilter($"This {email} is not valid");
            }

            string token = _generateHash.GenerateRandomNumber().ToString();

            string emailBody = $@"
                <div style='font-family: Arial, sans-serif; color: #333;'>
                    <h2 style='color: #0056b3;'>Redefinição da sua senha</h2>
                    <p>Olá,</p>
                    <p>Recebemos uma solicitação para redefinir sua senha. Use o código abaixo para verificar sua conta:</p>
                    <div style='text-align: center; margin: 20px 0;'>
                        <span style='font-size: 24px; font-weight: bold; color: #0056b3;'>{token}</span>
                    </div>
                    <p style='color: #777;'>Se você não solicitou essa redefinição, ignore este email.</p>
                    <p>Atenciosamente,</p>
                    <p>Equipe de Suporte</p>
                </div>";

            await _emailService.SendMail(
                  "no-reply@yourdomain.com", // Use a valid email address here
                  email,
                  "Redefinição da sua senha",
                  emailBody
               );

            _memoryCacheService.AddToCache(token, findEmail, 5);

            return token;
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<string> VerificationPasswordOTP(string token)
    {
        try
        {
            UserModel passwordResetCache = _memoryCacheService.GetCache<UserModel>(token);

            if (passwordResetCache == null)
            {
                throw new ExceptionFilter($"This token has expired");
            }

            string generateToken = _generateHash.GenerateToken(passwordResetCache);

            _memoryCacheService.RemoveFromCache<PasswordReset>(token);

            return generateToken;
        }
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<string> ResetPassword(PasswordReset passwordReset)
    {
        try
        {
            string clientId = _getClientIdToken.GetClientIdFromToken(_httpContext);

            UserModel user = await _authDao.GetIdAsync(clientId);

            if (user == null)
            {
                throw new ExceptionFilter($"This {clientId} is not valid");
            }

            var updateFields = new Dictionary<string, object>
            {
                { nameof(passwordReset.Password), _generateHash.GenerateHashParameters(passwordReset.Password) },
            };

            UserModel updatePassword = await _authDao.UpdateAsync(user.Id, updateFields);

            return "Senha Redefinida";
        }
        catch (ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<UserModel> RemoveAsync(string Id)
    {
        try
        {
            UserModel userView = await GetIdAsync(Id);

            var updateFields = new Dictionary<string, object>
            {
                { nameof(AccountStatus), AccountStatus.blocked },
            };

            UserModel updateUser = await _authDao.UpdateAsync(Id, updateFields);

            return updateUser;
        }
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }

    public async Task<UserModel> UpdateAsync(string Id, UserModel updateObject)
    {
        try
        {
            UserModel userId = await GetIdAsync(Id);

            var updateFields = new Dictionary<string, object>
            {
                { nameof(updateObject.UserName), updateObject.UserName },
                { nameof(updateObject.Password), updateObject.Password },
                { nameof(updateObject.AccountStatus), updateObject.AccountStatus }
            };

            UserModel userUpdate = await _authDao.UpdateAsync(Id, updateFields);

            return userUpdate;
        }
        catch(ExceptionFilter ex)
        {
            throw new ExceptionFilter(ex.Message, ex);
        }
    }
}