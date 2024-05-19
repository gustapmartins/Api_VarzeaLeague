using AutoFixture;
using MongoDB.Driver;
using Moq;
using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model.User;
using VarzeaLeague.Domain.Service;
using VarzeaLeague.Domain.Utils;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Test.Services;

public class AuthServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IAuthDao> _authDaoMock;
    private readonly Mock<IEmailService> _emailService;
    private readonly Mock<IMemoryCacheService> _memoryCacheService;
    private readonly IAuthService _authServiceMock;

    public AuthServiceTest()
    {
        _fixture = new Fixture();
        _authDaoMock = new Mock<IAuthDao>();
        _emailService = new Mock<IEmailService>();
        _memoryCacheService = new Mock<IMemoryCacheService>();
        _authServiceMock = new AuthService(_authDaoMock.Object, _emailService.Object, _memoryCacheService.Object);
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
    }

    [Fact]
    public async Task GetIdAsync_WhenNotAuthExist_ThrowsException()
    {
        _authDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((UserModel?)null);
        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.GetIdAsync(It.IsAny<string>()));

        Assert.Equal($"O usuário com o id '{It.IsAny<string>()}', não existe.", exception.Message);
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
    }

    [Fact]
    public async Task LoginAsync_WhenLogin_ReturnToken()
    {

        UserModel userMock = _fixture.Create<UserModel>();

        _authDaoMock.Setup(dao => dao.FindEmail(It.IsAny<string>())).ReturnsAsync((UserModel)null);

        bool isPasswordCorrect = GenerateHash.VerifyPassword(userMock.Password, userMock.Password); // Implemente esta função

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _authServiceMock.CreateAsync(userMock));

        //Assert
        Assert.Equal($"usuário com esse email: '{userMock.Email}', já existe.", exception.Message);
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
