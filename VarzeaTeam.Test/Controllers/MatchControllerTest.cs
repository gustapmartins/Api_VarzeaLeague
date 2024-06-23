using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VarzeaLeague.Application.DTO.Match;
using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.Controllers.v1;
using VarzeaTeam.Application.DTO.Match;

namespace VarzeaLeague.Test.Controllers;

public class MatchControllerTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IMatchService> _matchServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MatchController _matchController;

    public MatchControllerTest()
    {
        _fixture = new Fixture();
        _matchServiceMock = new Mock<IMatchService>();
        _mapperMock = new Mock<IMapper>();
        _matchController = new MatchController(_matchServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetMatch_WhenCalled_ReturnsOkResultWithMatchList()
    {
        // Arrange
        var matches = _fixture.CreateMany<MatchModel>(2).ToList();
        var matchViewDtos = _fixture.CreateMany<MatchViewDto>(2).ToList();

        _matchServiceMock.Setup(service => service.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterTypeEnum?>(), It.IsAny<string>(), It.IsAny<DateTime?>()))
            .ReturnsAsync(matches);

        _mapperMock.Setup(mapper => mapper.Map<List<MatchViewDto>>(It.IsAny<IEnumerable<MatchModel>>()))
            .Returns(matchViewDtos);

        // Act
        var result = await _matchController.GetMatch(1, 10) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<List<MatchViewDto>>(result.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetIdMatch_WhenCalled_ReturnsOkResultWithMatch()
    {
        // Arrange
        var match = _fixture.Create<MatchModel>();
        var matchViewDto = _fixture.Create<MatchViewDto>();

        _matchServiceMock.Setup(service => service.GetIdAsync(It.IsAny<string>()))
            .ReturnsAsync(match);

        _mapperMock.Setup(mapper => mapper.Map<MatchViewDto>(It.IsAny<MatchModel>()))
            .Returns(matchViewDto);

        // Act
        var result = await _matchController.GetIdMatch("someId") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<MatchViewDto>(result.Value);
        Assert.Equal(matchViewDto, returnValue);
    }

    [Fact]
    public async Task PostTeam_WhenCalled_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var matchCreateDto = _fixture.Create<MatchCreateDto>();
        var matchModel = _fixture.Create<MatchModel>();
        var matchViewDto = _fixture.Create<MatchViewDto>();

        _mapperMock.Setup(mapper => mapper.Map<MatchModel>(It.IsAny<MatchCreateDto>()))
            .Returns(matchModel);

        _matchServiceMock.Setup(service => service.CreateAsync(It.IsAny<MatchModel>()))
            .ReturnsAsync(matchModel);

        // Act
        var result = await _matchController.PostTeam(matchCreateDto) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task DeleteMatch_WhenCalled_ReturnsOkResultWithMatch()
    {
        // Arrange
        var match = _fixture.Create<MatchModel>();
        var matchViewDto = _fixture.Create<MatchViewDto>();

        _matchServiceMock.Setup(service => service.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(match);

        _mapperMock.Setup(mapper => mapper.Map<MatchViewDto>(It.IsAny<MatchModel>()))
            .Returns(matchViewDto);

        // Act
        var result = await _matchController.DeleteMatch("someId") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<MatchViewDto>(result.Value);
        Assert.Equal(matchViewDto, returnValue);
    }

    [Fact]
    public async Task UpdateMatch_WhenCalled_ReturnsOkResultWithUpdatedMatch()
    {
        // Arrange
        var matchUpdateDto = _fixture.Create<MatchUpdateDto>();
        var matchModel = _fixture.Create<MatchModel>();
        var matchViewDto = _fixture.Create<MatchViewDto>();

        _mapperMock.Setup(mapper => mapper.Map<MatchModel>(It.IsAny<MatchUpdateDto>()))
            .Returns(matchModel);

        _matchServiceMock.Setup(service => service.UpdateAsync(It.IsAny<string>(), It.IsAny<MatchModel>()))
            .ReturnsAsync(matchModel);

        _mapperMock.Setup(mapper => mapper.Map<MatchViewDto>(It.IsAny<MatchModel>()))
            .Returns(matchViewDto);

        // Act
        var result = await _matchController.UpdateMatch("someId", matchUpdateDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<MatchViewDto>(result.Value);
        Assert.Equal(matchViewDto, returnValue);
    }
}
