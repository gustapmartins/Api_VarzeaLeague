﻿using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaLeague.Domain.Service;
using VarzeaLeague.Domain.Utils;
using VarzeaTeam.Domain.Exceptions;
using AutoFixture;
using Moq;
using MongoDB.Driver;

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
        var filter = Builders<MatchModel>.Filter.Empty;

        _matchDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), filter)).ReturnsAsync(matchList);

        //Act
        var result = await _matchServiceMock.GetAsync(1, 10);

        // Assert
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
        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>()))!.ReturnsAsync((MatchModel?)null);
        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.GetIdAsync(It.IsAny<string>()));

        Assert.Equal($"A partida com o id '{It.IsAny<string>()}', não existe.", exception.Message);
    }

    [Fact]
    public async Task CreateMatch_WhenNewMatch_ReturnsMatch()
    {
        // Arrange
        var existingMatch = (MatchModel)null!;

        // Create the nested objects separately
        var homeTeam = _fixture.Build<TeamModel>().With(t => t.Id, "homeTeamId").Create();
        var visitingTeam = _fixture.Build<TeamModel>().With(t => t.Id, "visitingTeamId").Create();

        // Customize the MatchModel
        var matchToAdd = _fixture.Build<MatchModel>()
                                 .With(m => m.HomeTeamName, "homeTeamName")
                                 .With(m => m.VisitingTeamName, "visitingTeamName")
                                 .With(m => m.HomeTeamModel, homeTeam)
                                 .With(m => m.VisitingTeamModel, visitingTeam)
                                 .With(m => m.Local, "01001000")
                                 .Create();
        // Configure mocks
        _teamServiceMock.Setup(x => x.GetNameAsync(matchToAdd.HomeTeamName)).ReturnsAsync(homeTeam);
        _teamServiceMock.Setup(x => x.GetNameAsync(matchToAdd.VisitingTeamName)).ReturnsAsync(visitingTeam);

        _matchDaoMock.Setup(x => x.MatchExistsAsync(homeTeam.Id, visitingTeam.Id)).ReturnsAsync(existingMatch);

        // Mock the ViaCep call
        _matchDaoMock.Setup(x => x.CreateAsync(It.IsAny<MatchModel>())).Returns(Task.CompletedTask);
        var resultViaCep = await ViaCep.GetCep(matchToAdd.Local);


        // Act
        var createdMatch = await _matchServiceMock.CreateAsync(matchToAdd);

        // Assert
        _matchDaoMock.Verify(dao => dao.CreateAsync(It.Is<MatchModel>(m => m.HomeTeamModel.Id == homeTeam.Id && m.VisitingTeamModel.Id == visitingTeam.Id)), Times.Once);
        Assert.True(resultViaCep);  // Assegure-se de que o resultado é verdadeiro
        Assert.Equal(matchToAdd.HomeTeamModel.Id, createdMatch.HomeTeamModel.Id);
        Assert.Equal(matchToAdd.VisitingTeamModel.Id, createdMatch.VisitingTeamModel.Id);
    }

    [Fact]
    public async Task CreateMatch_WhenNewMatch_ThrowExceptionMatchExist()
    {
        // Arrange
        var homeTeam = _fixture.Build<TeamModel>().With(t => t.Id, "homeTeamId").Create();
        var visitingTeam = _fixture.Build<TeamModel>().With(t => t.Id, "visitingTeamId").Create();

        var matchToAdd = _fixture.Build<MatchModel>()
                                 .With(m => m.HomeTeamName, "homeTeamName")
                                 .With(m => m.VisitingTeamName, "visitingTeamName")
                                 .With(m => m.HomeTeamModel, homeTeam)
                                 .With(m => m.VisitingTeamModel, visitingTeam)
                                 .With(m => m.Local, "01001000")
                                 .Create();

        // Customize the MatchModel
        _teamServiceMock.Setup(x => x.GetNameAsync("homeTeamName")).ReturnsAsync(homeTeam);
        _teamServiceMock.Setup(x => x.GetNameAsync("visitingTeamName")).ReturnsAsync(visitingTeam);

        _matchDaoMock.Setup(dao => dao.MatchExistsAsync(matchToAdd.HomeTeamModel.Id, matchToAdd.VisitingTeamModel.Id))
                     .ReturnsAsync(matchToAdd);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.CreateAsync(matchToAdd));

        Assert.Equal("Já existe uma partida cadastrada com esses times", exception.Message);
    }

    [Fact]
    public async Task CreateMatch_WhenNewMatch_ThrowExceptionAndressExist()
    {
        // Arrange
        var existingMatch = (MatchModel)null!;
        // Arrange
        var homeTeam = _fixture.Build<TeamModel>().With(t => t.Id, "homeTeamId").Create();
        var visitingTeam = _fixture.Build<TeamModel>().With(t => t.Id, "visitingTeamId").Create();

        var matchToAdd = _fixture.Build<MatchModel>()
                                 .With(m => m.HomeTeamName, "homeTeamName")
                                 .With(m => m.VisitingTeamName, "visitingTeamName")
                                 .With(m => m.HomeTeamModel, homeTeam)
                                 .With(m => m.VisitingTeamModel, visitingTeam)
                                 .Create();

        _teamServiceMock.Setup(x => x.GetNameAsync("homeTeamName")).ReturnsAsync(homeTeam);
        _teamServiceMock.Setup(x => x.GetNameAsync("visitingTeamName")).ReturnsAsync(visitingTeam);


        _matchDaoMock.Setup(dao => dao.MatchExistsAsync(matchToAdd.HomeTeamModel.Id, matchToAdd.VisitingTeamModel.Id))
                     .ReturnsAsync(existingMatch);

        // Mock the ViaCep call to return false indicating the address does not exist
        var resultViaCep = await ViaCep.GetCep(matchToAdd.Local);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _matchServiceMock.CreateAsync(matchToAdd));

        Assert.Equal("Esse endereço não existe", exception.Message);
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
        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((MatchModel)null!);

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
        _matchDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync((MatchModel)null!);

        _matchDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () =>
            await _matchServiceMock.UpdateAsync(It.IsAny<string>(), It.IsAny<MatchModel>()));

        //Assert
        Assert.Equal($"A partida com o id '{It.IsAny<string>()}', não existe.", exception.Message);
    }

    [Fact]
    public async Task UpdateMatch_WhenTeamsAreNotTheSame_DoesNotThrowException()
    {
        // Arrange
        var existingHomeTeamName = "existingHomeTeam";
        var existingVisitingTeamName = "existingVisitingTeam";

        var existingMatch = _fixture.Build<MatchModel>()
                                    .With(m => m.HomeTeamName, existingHomeTeamName)
                                    .With(m => m.VisitingTeamName, existingVisitingTeamName)
                                    .Create();

        var updateObject = _fixture.Build<MatchModel>()
                                   .With(m => m.HomeTeamName, existingVisitingTeamName)
                                   .With(m => m.VisitingTeamName, existingHomeTeamName)
                                   .Create();

        _matchDaoMock.Setup(dao => dao.GetIdAsync("id")).ReturnsAsync(updateObject);
        _teamServiceMock.Setup(service => service.GetNameAsync(existingHomeTeamName)).ReturnsAsync(new TeamModel { NameTeam = existingHomeTeamName });
        _teamServiceMock.Setup(service => service.GetNameAsync(existingVisitingTeamName)).ReturnsAsync(new TeamModel { NameTeam = existingVisitingTeamName });

 


        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () =>
            await _matchServiceMock.UpdateAsync("id", existingMatch));

        //Assert
        Assert.Equal($"O mesmo time não pode jogar contra si mesmo.", exception.Message);
        Assert.Equal(existingVisitingTeamName, updateObject.HomeTeamName);
        Assert.Equal(existingHomeTeamName, updateObject.VisitingTeamName);
    }

}
