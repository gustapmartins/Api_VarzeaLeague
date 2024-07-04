using AutoFixture;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using Moq;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Domain.Exceptions;
using VarzeaTeam.Service;

namespace VarzeaLeague.Test.Services;

public class TeamServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IHttpContextAccessor> _httpContext;
    private readonly Mock<IGetClientIdToken> _getClientIdToken;
    private readonly Mock<ITeamDao> _teamDaoMock;
    private readonly TeamService _teamService;

    public TeamServiceTest()
    {
        _httpContext = new Mock<IHttpContextAccessor>();
        _teamDaoMock = new Mock<ITeamDao>();
        _getClientIdToken = new Mock<IGetClientIdToken>();
        _teamService = new TeamService(_teamDaoMock.Object, _httpContext.Object, _getClientIdToken.Object);
        _fixture = new Fixture();
    }

    [Fact]
    public async Task GetAsync_WhenTeamsExist_ReturnsTeamList()
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
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _teamService.GetAsync(1, 10));

        Assert.Equal($"N�o existe nenhum time cadastrado", exception.Message);
    }


    [Fact]
    public async Task GetNameAsync_WhenTeamsNameExist_ShouldReturnValid()
    {
        // Arrange
        TeamModel teamMock = _fixture.Build<TeamModel>()
                               .With(x => x.NameTeam, "NameValid")
                               .Create();

        _teamDaoMock.Setup(dao => dao.TeamExist(It.IsAny<string>()))
                   .ReturnsAsync(teamMock);

        // Act and Assert
        var result = await _teamService.GetNameAsync(It.IsAny<string>());

        Assert.NotNull(result);
        Assert.Equal(teamMock, result);
        _teamDaoMock.Verify(dao => dao.TeamExist(It.IsAny<string>()), Times.Once);
    }

    [Fact]
    public async Task GetNameAsync_WhenNoTeamsExist_ThrowsException()
    {
        // Arrange
        _teamDaoMock.Setup(dao => dao.TeamExist(It.IsAny<string>()))!.ReturnsAsync(null as TeamModel);

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _teamService.GetNameAsync(It.IsAny<string>()));

        Assert.Equal($"O Time com esse nome: {It.IsAny<string>()}, n�o existe.", exception.Message);
    }

    [Fact]
    public async Task GetIdAsync_WhenTeamsExist_ReturnsTeam()
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

    [Fact]
    public async Task GetIdAsync_WhenNotTeamsExist_ThrowsException()
    {

        string Id = It.IsAny<string>();

        _teamDaoMock.Setup(dao => dao.GetIdAsync(Id))!.ReturnsAsync((TeamModel?)null);
        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _teamService.GetIdAsync(Id));

        Assert.Equal($"O Time com o id {Id}, n�o existe.", exception.Message);
    }

    [Fact]
    public async Task CreateTeam_WhenNewTeam_ReturnsTeam()
    {
        // Arrange
        // Simula��o do HttpContext para simular a presen�a de um token JWT na solicita��o
        var httpContext = new DefaultHttpContext();
        httpContext.Request.Headers["Authorization"] = "Bearer valid_jwt_token_here";
        var httpContextAccessorMock = new Mock<IHttpContextAccessor>();
        httpContextAccessorMock.Setup(x => x.HttpContext).Returns(httpContext);

        var teamToAdd = _fixture.Build<TeamModel>()
                               .With(x => x.NameTeam, "NameTeam")
                               .With(x => x.ClientId, "ClientId")
                               .Create();

        // Configura��o do m�todo GetClientIdFromToken para retornar o ClientId desejado
        _getClientIdToken.Setup(x => x.GetClientIdFromToken(It.IsAny<HttpContext>())).Returns(teamToAdd.ClientId);

        var existingTeam = null as TeamModel; // Simulando que o time n�o existe

        _teamDaoMock.Setup(dao => dao.TeamExist(teamToAdd.NameTeam))!.ReturnsAsync(existingTeam);

        // Act
        var createdTeam = await _teamService.CreateAsync(teamToAdd);

        // Assert
        // Verifica se o m�todo CreateAsync foi chamado no DAO com o objeto de time correto
        _teamDaoMock.Verify(dao => dao.CreateAsync(teamToAdd), Times.Once);

        // Verifica se o ClientId foi configurado corretamente no objeto de time criado
        Assert.Equal(teamToAdd.ClientId, createdTeam.ClientId);
    }

    [Fact]
    public async Task CreateTeam_WhenNewTeam_ThrowException()
    {
        // Arrange
        var existingTeamName = "ExistingTeam";
        var teamToAdd = _fixture.Build<TeamModel>()
                               .With(x => x.NameTeam, existingTeamName)
                               .Create();

        _teamDaoMock.Setup(dao => dao.TeamExist(existingTeamName)).ReturnsAsync(teamToAdd);

        var HttpContext = new Mock<HttpContext>();
        _getClientIdToken.Setup(mock => mock.GetClientIdFromToken(HttpContext.Object)).Returns("ClientId");

        // Act & Assert
        await Assert.ThrowsAsync<ExceptionFilter>(() => _teamService.CreateAsync(teamToAdd));
    }

    [Fact]
    public async Task RemoveAsync_WhenTeamsExist_ReturnsTeam()
    {
        // Arrange
        TeamModel teamMock = _fixture.Create<TeamModel>();

        _teamDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(teamMock);

        _teamDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var removedTeam = await _teamService.RemoveAsync(It.IsAny<string>());

        //Assert
        Assert.Equal(teamMock, removedTeam);
    }

    [Fact]
    public async Task RemoveAsync_WhenNotTeamsExist_ThrowsException()
    {
        // Arrange
        _teamDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>()))!.ReturnsAsync(null as TeamModel);

        _teamDaoMock.Setup(dao => dao.RemoveAsync(It.IsAny<string>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _teamService.RemoveAsync(It.IsAny<string>()));

        //Assert
        Assert.Equal($"O Time com o id {It.IsAny<string>()}, n�o existe.", exception.Message);
    }

    [Fact]
    public async Task UpdateAsync_WhenTeamsExist_ReturnsTeam()
    {
        // Arrange
        TeamModel teamMock = _fixture.Create<TeamModel>();
        TeamModel UpadteTeamMock = _fixture.Create<TeamModel>();

        _teamDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>())).ReturnsAsync(teamMock);

        _teamDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()))
            .ReturnsAsync(teamMock);

        //Act
        var service = await _teamService.UpdateAsync(It.IsAny<string>(), UpadteTeamMock);

        //Assert
        Assert.NotNull(service);
        Assert.Equal(teamMock, service);

        _teamDaoMock.Verify(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_WhenNotTeamsExist_ThrowsException()
    {
        // Arrange
        _teamDaoMock.Setup(dao => dao.GetIdAsync(It.IsAny<string>()))!.ReturnsAsync(null as TeamModel);

        _teamDaoMock.Setup(dao => dao.UpdateAsync(It.IsAny<string>(), It.IsAny<Dictionary<string, object>>()));

        //Act
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => 
            await _teamService.UpdateAsync(It.IsAny<string>(), It.IsAny<TeamModel>()));

        //Assert
        Assert.Equal($"O Time com o id {It.IsAny<string>()}, n�o existe.", exception.Message);

    }
}