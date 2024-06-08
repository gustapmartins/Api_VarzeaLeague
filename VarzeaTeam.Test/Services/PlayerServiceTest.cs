using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Service;
using VarzeaTeam.Domain.Exceptions;
using MongoDB.Driver;
using AutoFixture;
using Moq;

namespace VarzeaLeague.Test.Services;

public class PlayerServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IPlayerDao> _playerDaoMock;
    private readonly Mock<ITeamService> _teamServiceMock;
    private readonly PlayerService _playerServiceMock;

    public PlayerServiceTest()
    {
        _fixture = new Fixture();
        _playerDaoMock = new Mock<IPlayerDao>();
        _teamServiceMock = new Mock<ITeamService>();
        _playerServiceMock = new PlayerService(_playerDaoMock.Object, _teamServiceMock.Object);
    }

    [Fact]
    public async Task GetAsync_WhenPlayersExist_ReturnsPlayerList()
    {
        // Arrange


        var playerList = _fixture.CreateMany<PlayerModel>(2).ToList();
        var teamModel = _fixture.Create<TeamModel>();

        _playerDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<FilterDefinition<PlayerModel>>())).ReturnsAsync(playerList);

        //Act
        var result = await _playerServiceMock.GetAsync(1, 10, "teamId");

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Collection(result,
                item => Assert.Equal(playerList[0].Id, item.Id),
                item => Assert.Equal(playerList[1].Id, item.Id)
            );
    }

    [Fact]
    public async Task GetAsync_WhenNoPlayerExist_ThrowsException()
    {
        // Arrange
        _playerDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterDefinition<PlayerModel>>()))
                   .ReturnsAsync(Enumerable.Empty<PlayerModel>());

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _playerServiceMock.GetAsync(1, 10, "teamId"));

        Assert.Equal($"Não existe nenhum player cadastrado", exception.Message);
    }

    [Fact]
    public async Task GetIdAsync_WhenPlayerExist_ReturnsPlayers()
    {
        // Arrange
        PlayerModel playerMock = _fixture.Create<PlayerModel>();

        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(playerMock);

        //Act
        var result = await _playerServiceMock.GetIdAsync(It.IsAny<string>());

        //Assert
        Assert.NotNull(result);
        Assert.Equal(playerMock, result);
    }

    [Fact]
    public async Task GetIdAsync_WhenNotPlayerExist_ThrowsException()
    {
        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(null as PlayerModel);
        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _playerServiceMock.GetIdAsync(It.IsAny<string>()));

        Assert.Equal($"O jogador com o id '{It.IsAny<string>()}' não existe.", exception.Message);
    }

    [Fact]
    public async Task CreatePlayer_WhenNewPlayer_ReturnsTeam()
    {
        // Arrange
        // Simulação do HttpContext para simular a presença de um token JWT na solicitação
        var existingTeam = null as PlayerModel;
        var playerToAdd = _fixture.Build<PlayerModel>()
                               .With(x => x.NamePlayer, "NameTeam")
                               .Create();

        var teamModel = _fixture.Create<TeamModel>();

        _teamServiceMock.Setup(x => x.GetIdAsync(It.IsAny<string>())).ReturnsAsync(teamModel);

        _playerDaoMock.Setup(dao => dao.PlayerExist(playerToAdd.NamePlayer)).ReturnsAsync(existingTeam);

        // Act
        var createdTeam = await _playerServiceMock.CreateAsync(playerToAdd);

        // Assert
        // Verifica se o método CreateAsync foi chamado no DAO com o objeto de time correto
        _playerDaoMock.Verify(dao => dao.CreateAsync(playerToAdd), Times.Once);

        // Verifica se o ClientId foi configurado corretamente no objeto de time criado
        Assert.Equal(playerToAdd.NamePlayer, createdTeam.NamePlayer);
    }

    [Fact]
    public async Task CreatePlayer_WhenNewPlayer_ThrowException()
    {
        // Arrange
        var existingTeamName = "ExistingTeam";
        var playerToAdd = _fixture.Build<PlayerModel>()
                               .With(x => x.NamePlayer, existingTeamName)
                               .Create();

        _playerDaoMock.Setup(dao => dao.PlayerExist(existingTeamName)).ReturnsAsync(playerToAdd);

        // Act & Assert

        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _playerServiceMock.CreateAsync(playerToAdd));

        Assert.Equal($"O jogador com o nome '{playerToAdd.NamePlayer}' já existe.", exception.Message);
    }

    [Fact]
    public async Task RemoveAsync_WhenPlayersExist_ReturnsPlayer()
    {
        // Arrange
        PlayerModel playerMock = _fixture.Create<PlayerModel>();

        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(playerMock);

        _playerDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var removedTeam = await _playerServiceMock.RemoveAsync(It.IsAny<string>());

        //Assert
        Assert.Equal(playerMock, removedTeam);
    }

    [Fact]
    public async Task RemoveAsync_WhenNotPlayersExist_ThrowsException()
    {
        // Arrange
        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(null as PlayerModel);

        _playerDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _playerServiceMock.RemoveAsync(It.IsAny<string>()));

        //Assert
        Assert.Equal($"O jogador com o id '{It.IsAny<string>()}' não existe.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenPlayersExist_ReturnsPlayer()
    {
        // Arrange
        PlayerModel playerMock = _fixture.Create<PlayerModel>();
        PlayerModel UpadteplayerMock = _fixture.Create<PlayerModel>();

        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(playerMock);

        _playerDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .ReturnsAsync(playerMock);

        //Act
        var service = await _playerServiceMock.UpdateAsync(It.IsAny<string>(), UpadteplayerMock);

        //Assert
        Assert.NotNull(service);
        Assert.Equal(playerMock, service);

        _playerDaoMock.Verify(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotPlayersExist_ThrowsException()
    {
        // Arrange
        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(null as PlayerModel);

        _playerDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => 
            await _playerServiceMock.UpdateAsync(It.IsAny<string>(), It.IsAny<PlayerModel>()));

        //Assert
        Assert.Equal($"O jogador com o id '{It.IsAny<string>()}' não existe.", exception.Message);

    }
}
