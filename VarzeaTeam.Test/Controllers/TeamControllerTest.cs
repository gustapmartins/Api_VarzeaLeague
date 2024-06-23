using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VarzeaLeague.Application.DTO.Team;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.Controllers.v1;
using VarzeaTeam.Application.DTO.Team;

namespace VarzeaLeague.Test.Controllers;

public class TeamControllerTest
{

    private readonly Fixture _fixture;
    private readonly Mock<ITeamService> _teamServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TeamController _teamController;

    public TeamControllerTest()
    {
        _fixture = new Fixture();
        _teamServiceMock = new Mock<ITeamService>();
        _mapperMock = new Mock<IMapper>();
        _teamController = new TeamController(_teamServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetTeams_WhenCalled_ReturnsOkResultWithTeamList()
    {
        // Arrange
        var teams = _fixture.CreateMany<TeamModel>(2).ToList();
        var teamViewDtos = _fixture.CreateMany<TeamViewDto>(2).ToList();

        _teamServiceMock.Setup(service => service.GetAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(teams);

        _mapperMock.Setup(mapper => mapper.Map<List<TeamViewDto>>(It.IsAny<IEnumerable<TeamModel>>()))
            .Returns(teamViewDtos);

        // Act
        var result = await _teamController.GetTeams(1, 10) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<List<TeamViewDto>>(result.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetIdTeams_WhenCalled_ReturnsOkResultWithTeam()
    {
        // Arrange
        var team = _fixture.Create<TeamModel>();
        var teamViewDto = _fixture.Create<TeamViewDto>();

        _teamServiceMock.Setup(service => service.GetIdAsync(It.IsAny<string>()))
            .ReturnsAsync(team);

        _mapperMock.Setup(mapper => mapper.Map<TeamViewDto>(It.IsAny<TeamModel>()))
            .Returns(teamViewDto);

        // Act
        var result = await _teamController.GetIdTeams("someId") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<TeamViewDto>(result.Value);
        Assert.Equal(teamViewDto, returnValue);
    }

    [Fact]
    public async Task CreateTeam_WhenCalled_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var teamCreateDto = _fixture.Create<TeamCreateDto>();
        var teamModel = _fixture.Create<TeamModel>();

        _mapperMock.Setup(mapper => mapper.Map<TeamModel>(It.IsAny<TeamCreateDto>()))
            .Returns(teamModel);

        _teamServiceMock.Setup(service => service.CreateAsync(It.IsAny<TeamModel>()))
            .ReturnsAsync(teamModel);

        // Act
        var result = await _teamController.CreateTeam(teamCreateDto) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task DeleteTeam_WhenCalled_ReturnsOkResultWithTeam()
    {
        // Arrange
        var team = _fixture.Create<TeamModel>();
        var teamViewDto = _fixture.Create<TeamViewDto>();

        _teamServiceMock.Setup(service => service.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(team);

        _mapperMock.Setup(mapper => mapper.Map<TeamViewDto>(It.IsAny<TeamModel>()))
            .Returns(teamViewDto);

        // Act
        var result = await _teamController.DeleteTeam("someId") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<TeamViewDto>(result.Value);
        Assert.Equal(teamViewDto, returnValue);
    }

    [Fact]
    public async Task UpdateTeam_WhenCalled_ReturnsOkResultWithUpdatedTeam()
    {
        // Arrange
        var teamUpdateDto = _fixture.Create<TeamUpdateDto>();
        var teamModel = _fixture.Create<TeamModel>();
        var teamViewDto = _fixture.Create<TeamViewDto>();

        _mapperMock.Setup(mapper => mapper.Map<TeamModel>(It.IsAny<TeamUpdateDto>()))
            .Returns(teamModel);

        _teamServiceMock.Setup(service => service.UpdateAsync(It.IsAny<string>(), It.IsAny<TeamModel>()))
            .ReturnsAsync(teamModel);

        _mapperMock.Setup(mapper => mapper.Map<TeamViewDto>(It.IsAny<TeamModel>()))
            .Returns(teamViewDto);

        // Act
        var result = await _teamController.UpdateTeam("someId", teamUpdateDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<TeamViewDto>(result.Value);
        Assert.Equal(teamViewDto, returnValue);
    }
}
