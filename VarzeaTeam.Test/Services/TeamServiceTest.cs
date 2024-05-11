using AutoFixture;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Moq;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Domain.Exceptions;
using VarzeaTeam.Service;

namespace VarzeaLeague.Test.Services;

public class TeamServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IHttpContextAccessor> _httpContext;
    private readonly Mock<ITeamDao> _teamDaoMock;
    private readonly TeamService _teamService;

    public TeamServiceTest()
    {
        _httpContext = new Mock<IHttpContextAccessor>();
        _teamDaoMock = new Mock<ITeamDao>();
        _teamService = new TeamService(_teamDaoMock.Object, _httpContext.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task FindAllTeam_WhenTeamsExist_ReturnsTeamList()
    {
        // Arrange
        var teamList = _fixture.CreateMany<TeamModel>(2).ToList();

        _teamDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterDefinition<TeamModel>>())).ReturnsAsync(teamList);

        //Act
        var result = await _teamService.GetAsync(1, 10);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Collection(result,
                item => Assert.Equal(teamList[0].Id, item.Id),
                item => Assert.Equal(teamList[1].Id, item.Id)
            );
    }

    [Fact]
    public async Task GetAsync_WhenNoTeamsExist_ThrowsException()
    {
        // Arrange

        _teamDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterDefinition<TeamModel>>()))
                   .ReturnsAsync(Enumerable.Empty<TeamModel>());

        // Act and Assert
        await Assert.ThrowsAsync<ExceptionFilter>(async () => await _teamService.GetAsync(1, 10));
    }

    [Fact]
    public async Task FindIdTeam_WhenTeamsExist_ReturnsTeam()
    {
        // Arrange
        TeamModel teamMock = _fixture.Create<TeamModel>();

        _teamDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(teamMock);

        //Act
        var result = await _teamService.GetIdAsync(It.IsAny<string>());

        //Assert
        Assert.NotNull(result);
        Assert.Equal(teamMock, result);
    }
}