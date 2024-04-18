using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaTeam.Domain.Exceptions;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Utils;
using Newtonsoft.Json.Linq;

namespace VarzeaLeague.Domain.Service;

public class AutheService : IAuthService
{
    private readonly IAuthDao _authDao;
    private readonly IEmailService _emailService;
    private readonly IMessagePublisher _messagePublisher;

    public AutheService(IAuthDao authDao, IMessagePublisher messagePublisher, IEmailService emailService)
    {
        _authDao = authDao;
        _messagePublisher = messagePublisher;
        _emailService = emailService;
    }

    public async Task<List<UserModel>> GetAsync(int page, int pageSize)
    {
        try
        {
            List<UserModel> GetAll = await _authDao.GetAsync(page, pageSize);

            if (GetAll.Count == 0)
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
                throw new ExceptionFilter($"Usuario com esse email: '{addObject.Email}', já existe.");

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
            var findEmail = await _authDao.FindEmail(email);

            if (findEmail == null)
            {
                throw new ExceptionFilter($"This {email} is not valid");
            }

            var token = GenerateHash.GenerateHashRandom();


            return "Um e-mail de redefinição de senha foi enviado para o seu endereço de e-mail";
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }


    public Task<UserModel> RemoveAsync(string Id)
    {
        try
        {

            throw new NotImplementedException();
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }

    public Task<UserModel> UpdateAsync(string Id, UserModel updateObject)
    {
        try
        {
            throw new NotImplementedException();
        }
        catch(Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
}
    