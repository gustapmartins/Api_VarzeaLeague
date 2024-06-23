using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VarzeaLeague.Application.Controllers.v1;
using VarzeaLeague.Application.DTO.Player;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;
using VarzeaTeam.Application.DTO.Player;

namespace VarzeaLeague.Test.Controllers;

public class PlayerControllerTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IPlayerService> _playerServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IMessagePublisher> _messagePublisherMock;
    private readonly PlayerController _playerController;

    public PlayerControllerTest()
    {
        _fixture = new Fixture();
        _playerServiceMock = new Mock<IPlayerService>();
        _mapperMock = new Mock<IMapper>();
        _messagePublisherMock = new Mock<IMessagePublisher>();
        _playerController = new PlayerController(_playerServiceMock.Object, _mapperMock.Object, _messagePublisherMock.Object);
    }

    [Fact]
    public async Task GetPlayer_WhenCalled_ReturnsOkResultWithPlayerList()
    {
        // Arrange
        var players = _fixture.CreateMany<PlayerModel>(2).ToList();
        var playerViewDtos = _fixture.CreateMany<PlayerViewDto>(2).ToList();

        _playerServiceMock.Setup(service => service.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string>()))
            .ReturnsAsync(players);

        _mapperMock.Setup(mapper => mapper.Map<List<PlayerViewDto>>(It.IsAny<IEnumerable<PlayerModel>>()))
            .Returns(playerViewDtos);

        // Act
        var result = await _playerController.GetPlayer(1, 10) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<List<PlayerViewDto>>(result.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetIdPlayer_WhenCalled_ReturnsOkResultWithPlayer()
    {
        // Arrange
        var player = _fixture.Create<PlayerModel>();
        var playerViewDto = _fixture.Create<PlayerViewDto>();

        _playerServiceMock.Setup(service => service.GetIdAsync(It.IsAny<string>()))
            .ReturnsAsync(player);

        _mapperMock.Setup(mapper => mapper.Map<PlayerViewDto>(It.IsAny<PlayerModel>()))
            .Returns(playerViewDto);

        // Act
        var result = await _playerController.GetIdPlayer("someId") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<PlayerViewDto>(result.Value);
        Assert.Equal(playerViewDto, returnValue);
    }

    [Fact]
    public async Task CreatePlayer_WhenCalled_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var playerCreateDto = _fixture.Create<PlayerCreateDto>();
        var playerModel = _fixture.Create<PlayerModel>();

        _mapperMock.Setup(mapper => mapper.Map<PlayerModel>(It.IsAny<PlayerCreateDto>()))
            .Returns(playerModel);

        _playerServiceMock.Setup(service => service.CreateAsync(It.IsAny<PlayerModel>()))
            .ReturnsAsync(playerModel);

        // Act
        var result = await _playerController.CreatePlayer(playerCreateDto) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task DeletePlayer_WhenCalled_ReturnsOkResultWithPlayer()
    {
        // Arrange
        var player = _fixture.Create<PlayerModel>();
        var playerViewDto = _fixture.Create<PlayerViewDto>();

        _playerServiceMock.Setup(service => service.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(player);

        _mapperMock.Setup(mapper => mapper.Map<PlayerViewDto>(It.IsAny<PlayerModel>()))
            .Returns(playerViewDto);

        // Act
        var result = await _playerController.DeletePlayer("someId") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<PlayerViewDto>(result.Value);
        Assert.Equal(playerViewDto, returnValue);
    }

    [Fact]
    public async Task UpdatePlayer_WhenCalled_ReturnsOkResultWithUpdatedPlayer()
    {
        // Arrange
        var playerUpdateDto = _fixture.Create<PlayerUpdateDto>();
        var playerModel = _fixture.Create<PlayerModel>();
        var playerViewDto = _fixture.Create<PlayerViewDto>();

        _mapperMock.Setup(mapper => mapper.Map<PlayerModel>(It.IsAny<PlayerUpdateDto>()))
            .Returns(playerModel);

        _playerServiceMock.Setup(service => service.UpdateAsync(It.IsAny<string>(), It.IsAny<PlayerModel>()))
            .ReturnsAsync(playerModel);

        _mapperMock.Setup(mapper => mapper.Map<PlayerViewDto>(It.IsAny<PlayerModel>()))
            .Returns(playerViewDto);

        // Act
        var result = await _playerController.UpdatePlayer("someId", playerUpdateDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<PlayerViewDto>(result.Value);
        Assert.Equal(playerViewDto, returnValue);
    }

    [Fact]
    public async Task ProduceAsync_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        _messagePublisherMock.Setup(publisher => publisher.ProduceAsync(It.IsAny<string>()))
            .Returns(Task.CompletedTask);

        // Act
        var result = await _playerController.ProduceAsync() as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<string>(result.Value);
        Assert.Equal("Mensagem enviada para o Kafka.", returnValue);
    }
}
