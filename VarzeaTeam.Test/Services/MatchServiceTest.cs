using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Service;
using VarzeaLeague.Domain.Utils;
using VarzeaTeam.Domain.Exceptions;
using AutoFixture;
using Moq;

namespace VarzeaLeague.Test.Services;

public class MatchServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IMatchDao> _matchDaoMock;
    private readonly Mock<ITeamService> _teamServiceMock;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly MatchService _matchServiceMock;

    public MatchServiceTest()
    {
        _fixture = new Fixture();
        _matchDaoMock = new Mock<IMatchDao>();
        _teamServiceMock = new Mock<ITeamService>();
        _notificationServiceMock = new Mock<INotificationService>();
        _matchServiceMock = new MatchService(_matchDaoMock.Object, _teamServiceMock.Object, _notificationServiceMock.Object);
    }

    [Fact]
    public async Task GetAsync_WhenPlayersExist_ReturnsPlayerList()
    {
        // Arrange
        var matchList = _fixture.CreateMany<MatchModel>(2).ToList();
        var matchModel = _fixture.Create<MatchModel>();

        _matchDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), null)).ReturnsAsync(matchList);

        //Act
        var result = await _matchServiceMock.GetAsync(1, 10);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Collection(result,
                item => Assert.Equal(matchList[0].Id, item.Id),
                item => Assert.Equal(matchList[1].Id, item.Id)
            );
    }

    [Fact]
    public async Task GetAsync_WhenNoPlayerExist_ThrowsException()
    {
        // Arrange
        _matchDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), null))
                   .ReturnsAsync(Enumerable.Empty<MatchModel>());

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.GetAsync(1, 10));

        Assert.Equal($"Não existe nenhuma partida cadastrada", exception.Message);
    }

    [Fact]
    public async Task GetIdAsync_WhenTeamsExist_ReturnsTeam()
    {
        // Arrange
        MatchModel matchMock = _fixture.Create<MatchModel>();

        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(matchMock);

        //Act
        var result = await _matchServiceMock.GetIdAsync(It.IsAny<string>());

        //Assert
        Assert.NotNull(result);
        Assert.Equal(matchMock, result);
    }

    [Fact]
    public async Task GetIdAsync_WhenNotTeamsExist_ThrowsException()
    {
        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((MatchModel?)null);
        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.GetIdAsync(It.IsAny<string>()));

        Assert.Equal($"A partida com o id '{It.IsAny<string>()}', não existe.", exception.Message);
    }

    [Fact]
    public async Task CreateMatch_WhenNewMatch_ReturnsMatch()
    {
        // Arrange
        var existingMatch = (MatchModel)null;
        var matchToAdd = _fixture.Build<MatchModel>()
                                .With(x => x.HomeTeamId, "homeTeamId")
                                .With(x => x.VisitingTeamId, "visitingTeamId")
                                .With(x => x.Local, "01001000")
                                .Create();

        var teamModel = _fixture.Create<TeamModel>();

        _teamServiceMock.Setup(x => x.GetIdAsync("homeTeamId")).ReturnsAsync(teamModel);
        _teamServiceMock.Setup(x => x.GetIdAsync("visitingTeamId")).ReturnsAsync(teamModel);

        _matchDaoMock.Setup(x => x.MatchExistsAsync("homeTeamId", "visitingTeamId")).ReturnsAsync(existingMatch);

        var resultViaCep = await ViaCep.GetCep(matchToAdd.Local);

        _matchDaoMock.Setup(x => x.CreateAsync(It.IsAny<MatchModel>())).Returns(Task.CompletedTask);

        // Act
        var createdMatch = await _matchServiceMock.CreateAsync(matchToAdd);

        // Assert
        _matchDaoMock.Verify(dao => dao.CreateAsync(It.Is<MatchModel>(m => m.HomeTeamId == matchToAdd.HomeTeamId && m.VisitingTeamId == matchToAdd.VisitingTeamId)), Times.Once);
        Assert.Equal(matchToAdd.HomeTeamId, createdMatch.HomeTeamId);
        Assert.Equal(matchToAdd.VisitingTeamId, createdMatch.VisitingTeamId);
        Assert.True(resultViaCep);
    }

    [Fact]
    public async Task CreateMatch_WhenNewMatch_ThrowExceptionMatchExist()
    {
        // Arrange
        var matchToAdd = _fixture.Build<MatchModel>()
                               .With(x => x.HomeTeamId, "HomeTeamId")
                               .With(x => x.VisitingTeamId, "visitingId")
                               .Create();

        _matchDaoMock.Setup(dao => dao.MatchExistsAsync(matchToAdd.HomeTeamId, matchToAdd.VisitingTeamId)).ReturnsAsync(matchToAdd);

        // Act & Assert

        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.CreateAsync(matchToAdd));

        Assert.Equal($"Já existe uma partida cadastrada com esses times", exception.Message);
    }

    [Fact]
    public async Task CreateMatch_WhenNewMatch_ThrowExceptionAndressExist()
    {
        // Arrange
        var existingMatch = (MatchModel)null;
        var matchToAdd = _fixture.Build<MatchModel>()
                               .With(x => x.HomeTeamId, "HomeTeamId")
                               .With(x => x.VisitingTeamId, "visitingId")
                               .Create();

        _matchDaoMock.Setup(dao => dao.MatchExistsAsync(matchToAdd.HomeTeamId, matchToAdd.VisitingTeamId)).ReturnsAsync(existingMatch);

        var resultViaCep = await ViaCep.GetCep(matchToAdd.Local);
        // Act & Assert

        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.CreateAsync(matchToAdd));

        Assert.Equal($"Esse endereço não existe", exception.Message);
        Assert.False(resultViaCep);
    }

    [Fact]
    public async Task RemoveAsync_WhenTeamsExist_ReturnsTeam()
    {
        // Arrange
        MatchModel matchMock = _fixture.Create<MatchModel>();

        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(matchMock);

        _matchDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var removedMatch = await _matchServiceMock.RemoveAsync(It.IsAny<string>());

        //Assert
        Assert.Equal(matchMock, removedMatch);
    }

    [Fact]
    public async Task RemoveAsync_WhenNotTeamsExist_ThrowsException()
    {
        // Arrange
        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((MatchModel)null);

        _matchDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.RemoveAsync(It.IsAny<string>()));

        //Assert
        Assert.Equal($"A partida com o id '{It.IsAny<string>()}', não existe.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenPlayersExist_ReturnsPlayer()
    {
        // Arrange
        MatchModel matchMock = _fixture.Create<MatchModel>();
        MatchModel upadteMatchMock = _fixture.Create<MatchModel>();

        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(matchMock);

        _matchDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .ReturnsAsync(matchMock);

        //Act
        var service = await _matchServiceMock.UpdateAsync(It.IsAny<string>(), upadteMatchMock);

        //Assert
        Assert.NotNull(service);
        Assert.Equal(matchMock, service);

        _matchDaoMock.Verify(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotTeamsExist_ThrowsException()
    {
        // Arrange
        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((MatchModel)null);

        _matchDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => 
            await _matchServiceMock.UpdateAsync(It.IsAny<string>(), It.IsAny<MatchModel>()));

        //Assert
        Assert.Equal($"A partida com o id '{It.IsAny<string>()}', não existe.", exception.Message);
    }
}
