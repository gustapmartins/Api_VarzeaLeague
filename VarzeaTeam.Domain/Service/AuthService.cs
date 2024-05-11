﻿using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaTeam.Domain.Exceptions;
using VarzeaLeague.Domain.Utils;
using VarzeaLeague.Domain.Model.User;
using MongoDB.Driver;
using VarzeaLeague.Domain.Enum;

namespace VarzeaLeague.Domain.Service;

public class AuthService : IAuthService
{
    private readonly IAuthDao _authDao;
    private readonly IMemoryCacheService _memoryCacheService;
    private readonly IEmailService _emailService;
    private readonly IMessagePublisher _messagePublisher;

    public AuthService(IAuthDao authDao, IMessagePublisher messagePublisher, IEmailService emailService, IMemoryCacheService memoryCacheService)
    {
        _authDao = authDao;
        _messagePublisher = messagePublisher;
        _emailService = emailService;
        _memoryCacheService = memoryCacheService;
    }

    public async Task<IEnumerable<UserModel>> GetAsync(int page, int pageSize)
    {
        try
        {
            IEnumerable<UserModel> GetAll = await _authDao.GetAsync(page, pageSize, filter: Builders<UserModel>.Filter.Where(x => x.AccountStatus == AccountStatus.active));

            if (GetAll.Count() == 0)
                throw new ExceptionFilter($"Não existe nenhuma partida cadastrada");

            return GetAll;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserModel> GetIdAsync(string Id)
    {
        try
        {
            UserModel GetId = await _authDao.GetIdAsync(Id);

            if (GetId == null)
                throw new ExceptionFilter($"A partida com o id '{Id}', não existe.");

            if(GetId.AccountStatus == 0)
                throw new ExceptionFilter($"Esse usuario '{Id}' está bloqueado.");

            return GetId;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<string> Login(UserModel userLogin)
    {
        try
        {
            UserModel findUser = await _authDao.FindEmail(userLogin.Email);

            if (findUser == null)
                throw new ExceptionFilter($"This {userLogin.Email} not exists");

            //Faz uma validação para verificar se a senha que o usuariop está passando corresponde a senha salva no banco, em formato hash
            bool isPasswordCorrect = GenerateHash.VerifyPassword(userLogin.Password, findUser.Password); // Implemente esta função

            if (!isPasswordCorrect)
                throw new UnauthorizedAccessException("Incorrect password");

            //Gera um token a partir do usuario buscado pelo E-mail
            var token = GenerateHash.GenerateToken(findUser);

            return token;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<UserModel> CreateAsync(UserModel addObject)
    {
        try
        {
            UserModel findEmail = await _authDao.FindEmail(addObject.Email);

            if (findEmail != null)
                throw new ExceptionFilter($"user with this email: '{addObject.Email}', already exists.");

            addObject.Password = GenerateHash.GenerateHashParameters(addObject.Password);

            UserModel user = new()
            {
                UserName = addObject.UserName,
                Email = addObject.Email,
                Password = addObject.Password,
                Cpf = addObject.Cpf,
                Role = addObject.Role,
                AccountStatus = Enum.AccountStatus.active,
                DateCreated = DateTime.Now
            };

            await _authDao.CreateAsync(user);

            return user;
        }
        catch(Exception ex) 
        {
           throw new Exception(ex.Message, ex);
        }
    }

    public async Task<string> ForgetPassword(string email)
    {
        try
        {
            UserModel findEmail = await _authDao.FindEmail(email);

            if (findEmail == null)
                throw new ExceptionFilter($"This {email} is not valid");

            var token = GenerateHash.GenerateHashRandom();

            var PasswordReset = new PasswordReset
            {
                Token = token,
                Email = email,   
            };

            _emailService.SendMail(
                      email,
                      "Redefinição da sua senha",
                      $"Verifique sua conta, com essa token: {token}"
                   );

            _memoryCacheService.AddToCache(token, PasswordReset, 5); //5 minutos de cache

            return token;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<string> ResetPassword(PasswordReset passwordReset)
    {
        try
        {
            PasswordReset passwordResetCache = _memoryCacheService.GetCache<PasswordReset>(passwordReset.Token);

            if(passwordResetCache == null)
                throw new ExceptionFilter($"This token has expired");

            UserModel findEmail = await _authDao.FindEmail(passwordResetCache.Email);

            if(findEmail == null)
                throw new ExceptionFilter($"This {passwordReset.Email} is not valid");

            UserModel userlUpdatePassword = new()
            {
                Password = passwordReset.Password,
            };

            var updateFields = new Dictionary<string, object>
            {
                { nameof(passwordReset.Password), passwordReset.Password },
                // Adicione outros campos que deseja atualizar conforme necessário
            };

            UserModel updatePassword = await _authDao.UpdateAsync(findEmail.Id, updateFields);

            _memoryCacheService.RemoveFromCache<PasswordReset>(passwordReset.Token);

            return "Senha redefinida";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<UserModel> RemoveAsync(string Id)
    {
        try
        {
            UserModel userView = await _authDao.GetIdAsync(Id);

            var updateFields = new Dictionary<string, object>
            {
                { nameof(AccountStatus),AccountStatus.blocked },
                // Adicione outros campos que deseja atualizar conforme necessário
            };

            UserModel userRemove = new()
            {
                AccountStatus = AccountStatus.blocked
            };

            await _authDao.UpdateAsync(Id, updateFields);

            return userView;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public async Task<UserModel> UpdateAsync(string Id, UserModel updateObject)
    {
        try
        {
            UserModel userId = await _authDao.GetIdAsync(Id);

            var updateFields = new Dictionary<string, object>
            {
                { nameof(updateObject.UserName), updateObject.UserName },
                { nameof(updateObject.Password), updateObject.Password },
                { nameof(updateObject.AccountStatus), updateObject.AccountStatus }
                // Adicione outros campos que deseja atualizar conforme necessário
            };


            UserModel userUpdate = await _authDao.UpdateAsync(Id, updateFields);

            return userUpdate;
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
    