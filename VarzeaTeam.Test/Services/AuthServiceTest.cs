using AutoFixture;
using MongoDB.Driver;
using Moq;
using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Model.User;
using VarzeaLeague.Domain.Service;
using VarzeaLeague.Domain.Utils;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Test.Services;

public class AuthServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IAuthDao> _authDaoMock;
    private readonly Mock<IEmailService> _emailServiceMock;
    private readonly Mock<IMemoryCacheService> _memoryCacheServiceMock;
    private readonly Mock<IGenerateHash> _generateHashMock;
    private readonly IAuthService _authServiceMock;

    public AuthServiceTest()
    {
        _fixture = new Fixture();
        _authDaoMock = new Mock<IAuthDao>();
        _emailServiceMock = new Mock<IEmailService>();
        _memoryCacheServiceMock = new Mock<IMemoryCacheService>();
        _generateHashMock = new Mock<IGenerateHash>();
        _authServiceMock = new AuthService(_authDaoMock.Object, _emailServiceMock.Object, _memoryCacheServiceMock.Object, _generateHashMock.Object);
    }

    [Fact]
    public async Task GetAsync_WhenAuthExist_ReturnsAuthList()
    {
        // Arrange
        List<UserModel> userList = _fixture.CreateMany<UserModel>(2).ToList();
        UserModel userModel = _fixture.Create<UserModel>();

        _authDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterDefinition<UserModel>>())).ReturnsAsync(userList);

        //Act
        var result = await _authServiceMock.GetAsync(1, 10);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Collection(result,
                item => Assert.Equal(userList[0].Id, item.Id),
                item => Assert.Equal(userList[1].Id, item.Id)
            );
        _authDaoMock.Verify(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterDefinition<UserModel>>()), Times.Once);
    }

    [Fact]
    public async Task GetAsync_WhenNoAuthsExist_ThrowsException()
    {
        // Arrange
        _authDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), null))
                   .ReturnsAsync(Enumerable.Empty<UserModel>());

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.GetAsync(1, 10));

        Assert.Equal($"Não existe nenhum usuário cadastrada", exception.Message);
    }

    [Fact]
    public async Task GetIdAsync_WhenAuthExist_ReturnsAuth()
    {
        // Arrange
        UserModel userMock = _fixture.Build<UserModel>()
                               .With(x => x.AccountStatus, AccountStatus.active)
                               .Create();

        _authDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(userMock);

        //Act
        var result = await _authServiceMock.GetIdAsync(It.IsAny<string>());

        //Assert
        Assert.NotNull(result);
        Assert.Equal(userMock, result);
        _authDaoMock.Verify(dao => dao.GetIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetIdAsync_WhenNotAuthExist_ThrowsException()
    {
        _authDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((UserModel?)null);
        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.GetIdAsync(It.IsAny<string>()));

        Assert.Equal($"O usuário com o id '{It.IsAny<string>()}', não existe.", exception.Message);
        _authDaoMock.Verify(dao => dao.GetIdAsync(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenAuthExist_ReturnsAuth()
    {

        UserModel userMock = _fixture.Build<UserModel>()
                               .With(x => x.Id, string.Empty)
                               .With(x => x.AccountStatus, AccountStatus.active)
                               .Create();

        _authDaoMock.Setup(dao => dao.FindEmail(It.IsAny<string>())).ReturnsAsync((UserModel)null);

        _authDaoMock.Setup(dao => dao.CreateAsync(It.IsAny<UserModel>()));

        //Act
        var createdUser = await _authServiceMock.CreateAsync(userMock);

        //Assert
        Assert.Equal(userMock, createdUser);
        _authDaoMock.Verify(dao => dao.FindEmail(It.IsAny<string>()), Times.Once);
        _authDaoMock.Verify(dao => dao.CreateAsync(It.IsAny<UserModel>()), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_WhenAuthExist_ThrowsException()
    {

        UserModel userMock = _fixture.Create<UserModel>();

        _authDaoMock.Setup(dao => dao.FindEmail(It.IsAny<string>())).ReturnsAsync(userMock);

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.CreateAsync(userMock));

        //Assert
        Assert.Equal($"usuário com esse email: '{userMock.Email}', já existe.", exception.Message);
        _authDaoMock.Verify(dao => dao.FindEmail(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task LoginAsync_WhenLogin_ReturnToken()
    {
        //Arrange
        UserModel userMock = _fixture.Create<UserModel>();
        string token = _fixture.Create<string>();

        _authDaoMock.Setup(dao => dao.FindEmail(It.IsAny<string>())).ReturnsAsync(userMock);

        _generateHashMock.Setup(hash => hash.VerifyPassword(userMock.Password, userMock.Password)).Returns(true);

        _generateHashMock.Setup(hash => hash.GenerateToken(userMock)).Returns(token);

        //Act
        var result = await _authServiceMock.Login(userMock);

        //Assert
        Assert.Equal(token, result);
        _authDaoMock.Verify(dao => dao.FindEmail(It.IsAny<string>()), Times.Once);
        _generateHashMock.Verify(hash => hash.VerifyPassword(userMock.Password, userMock.Password), Times.Once);
        _generateHashMock.Verify(hash => hash.GenerateToken(userMock), Times.Once);
    }

    [Fact]
    public async Task LoginFindEmailNotExistAsync_WhenLogin_ThrowsException()
    {
        //Arranges
        UserModel userMock = _fixture.Create<UserModel>();
        string token = _fixture.Create<string>();

        _authDaoMock.Setup(dao => dao.FindEmail(It.IsAny<string>())).ReturnsAsync((UserModel)null);

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.Login(userMock));

        //Assert
        Assert.Equal($"Este email: {userMock.Email} não existe.", exception.Message);
        _authDaoMock.Verify(dao => dao.FindEmail(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task LoginVerifyPasswordAsync_WhenLogin_ThrowsException()
    {
        //Arrange
        UserModel userMock = _fixture.Create<UserModel>();
        string token = _fixture.Create<string>();

        _authDaoMock.Setup(dao => dao.FindEmail(It.IsAny<string>())).ReturnsAsync(userMock);

        _generateHashMock.Setup(hash => hash.VerifyPassword(userMock.Password, userMock.Password)).Returns(false);

        //Act
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(async () => await _authServiceMock.Login(userMock));

        //Assert
        Assert.Equal($"Senha incorreta", exception.Message);
        _authDaoMock.Verify(dao => dao.FindEmail(It.IsAny<string>()), Times.Once);
        _generateHashMock.Verify(hash => hash.VerifyPassword(userMock.Password, userMock.Password), Times.Once);
    }

    [Fact]
    public async Task ForgetPassword_WhenEmailIsValid_SendsEmailAndStoresTokenInCache()
    {
        // Arrange
        string email = "test@example.com";
        string token = "generatedToken";
        var user = _fixture.Create<UserModel>();

        _authDaoMock.Setup(dao => dao.FindEmail(email)).ReturnsAsync(user);
        _generateHashMock.Setup(hash => hash.GenerateHashRandom()).Returns(token);

        // Act
        var result = await _authServiceMock.ForgetPassword(email);

        // Assert
        Assert.Equal(token, result);
        _emailServiceMock.Verify(service => service.SendMail(email, It.IsAny<string>(), It.Is<string>(msg => msg.Contains(token))), Times.Once);
        _memoryCacheServiceMock.Verify(cache => cache.AddToCache(token, It.IsAny<PasswordReset>(), 5), Times.Once);
    }

    [Fact]
    public async Task ForgetPassword_WhenEmailIsInvalid_ThrowsException()
    {
        // Arrange
        string email = "invalid@example.com";
        _authDaoMock.Setup(dao => dao.FindEmail(email)).ReturnsAsync((UserModel)null);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(() => _authServiceMock.ForgetPassword(email));
        Assert.Equal($"This {email} is not valid", exception.Message);
    }

    [Fact]
    public async Task ResetPassword_WhenTokenIsValid_UpdatesPasswordAndRemovesTokenFromCache()
    {
        // Arrange
        string token = "validToken";
        string email = "test@example.com";
        string newPassword = "newPassword";
        var passwordReset = new PasswordReset { Token = token, Email = email };
        var user = _fixture.Create<UserModel>();

        _memoryCacheServiceMock.Setup(cache => cache.GetCache<PasswordReset>(token)).Returns(passwordReset);
        _authDaoMock.Setup(dao => dao.FindEmail(email)).ReturnsAsync(user);
        _authDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>())).ReturnsAsync(user);

        // Act
        var result = await _authServiceMock.ResetPassword(new PasswordReset { Token = token, Password = newPassword });

        // Assert
        Assert.Equal("Senha redefinida", result);
        _memoryCacheServiceMock.Verify(cache => cache.RemoveFromCache<PasswordReset>(token), Times.Once);
        _authDaoMock.Verify(dao => dao.UpdateAsync(user.Id, It.IsAny<Dictionary<string, object>>()), Times.Once);
    }

    [Fact]
    public async Task ResetPassword_WhenTokenIsInvalid_ThrowsException()
    {
        // Arrange
        string token = "invalidToken";
        _memoryCacheServiceMock.Setup(cache => cache.GetCache<PasswordReset>(token)).Returns((PasswordReset)null);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(() => _authServiceMock.ResetPassword(new PasswordReset { Token = token }));
        Assert.Equal("This token has expired", exception.Message);
    }

    [Fact]
    public async Task RemoveAsync_WhenTeamsExist_ReturnsTeam()
    {
        UserModel userMock = _fixture.Build<UserModel>()
                               .With(x => x.AccountStatus, AccountStatus.active)
                               .Create();

        _authDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(userMock);

        _authDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
             .ReturnsAsync(userMock);

        //Act
        var removedTeam = await _authServiceMock.RemoveAsync(It.IsAny<string>());

        //Assert
        Assert.Equal(userMock, removedTeam);
    }

    [Fact]
    public async Task RemoveAsync_WhenNotAuthExist_ThrowsException()
    {
        // Arrange
        _authDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((UserModel)null);

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.RemoveAsync(It.IsAny<string>()));

        //Assert
        Assert.Equal($"O usuário com o id '{It.IsAny< string>()}', não existe.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenAuthExist_ReturnsAuth()
    {
        // Arrange
        UserModel userMock = _fixture.Build<UserModel>()
                                .With(x => x.AccountStatus, AccountStatus.active)
                                .Create();
        UserModel upadteUserMock = _fixture.Build<UserModel>()
                                .With(x => x.AccountStatus, AccountStatus.blocked)
                                .Create(); ;

        _authDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(userMock);

        _authDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .ReturnsAsync(upadteUserMock);

        //Act
        var service = await _authServiceMock.UpdateAsync(It.IsAny<string>(), upadteUserMock);

        //Assert
        Assert.NotNull(service);
        Assert.Equal(upadteUserMock, service);

        _authDaoMock.Verify(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotAuthExist_ThrowsException()
    {
        // Arrange
        _authDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((UserModel)null);

        _authDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.RemoveAsync(It.IsAny<string>()));

        //Assert
        Assert.Equal($"O usuário com o id '{It.IsAny<string>()}', não existe.", exception.Message);

    }
}
