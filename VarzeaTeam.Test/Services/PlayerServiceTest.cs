using Microsoft.AspNetCore.Http;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Service;
using VarzeaTeam.Domain.Exceptions;
using AutoFixture;
using MongoDB.Driver;
using Moq;
using Nest;

namespace VarzeaLeague.Test.Services;

public class PlayerServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IPlayerDao> _playerDaoMock;
    private readonly Mock<ITeamDao> _teamDaoMock;
    private readonly PlayerService _playerServiceMock;

    public PlayerServiceTest()
    {
        _fixture = new Fixture();
        _playerDaoMock = new Mock<IPlayerDao>();
        _teamDaoMock = new Mock<ITeamDao>();
        _playerServiceMock = new PlayerService(_playerDaoMock.Object, _teamDaoMock.Object);
    }

    [Fact]
    public async Task GetAsync_WhenPlayersExist_ReturnsPlayerList()
    {
        // Arrange


        var playerList = _fixture.CreateMany<PlayerModel>(2).ToList();
        var teamModel = _fixture.Create<TeamModel>();

        _teamDaoMock.Setup(dao => dao.GetIdAsync("teamId")).ReturnsAsync(teamModel);

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

        string Id = It.IsAny<string>();

        _playerDaoMock.Setup(dao => dao.GetIdAsync(Id)).ReturnsAsync((PlayerModel?)null);
        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _playerServiceMock.GetIdAsync(Id));

        Assert.Equal($"O jogador com o id '{Id}' não existe.", exception.Message);
    }

    [Fact]
    public async Task RemoveAsync_WhenTeamsExist_ReturnsTeam()
    {
        // Arrange
        PlayerModel teamMock = _fixture.Create<PlayerModel>();

        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(teamMock);

        _playerDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var removedTeam = await _playerServiceMock.RemoveAsync(It.IsAny<string>());

        //Assert
        Assert.Equal(teamMock, removedTeam);
    }

    [Fact]
    public async Task RemoveAsync_WhenNotTeamsExist_ThrowsException()
    {
        // Arrange
        _playerDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((PlayerModel)null);

        _playerDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _playerServiceMock.RemoveAsync(It.IsAny<string>()));

        //Assert
        Assert.Equal($"O jogador com o id '{It.IsAny<string>()}' não existe.", exception.Message);
    }
}
