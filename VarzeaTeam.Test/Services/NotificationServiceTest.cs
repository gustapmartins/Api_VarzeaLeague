using AutoFixture;
using Microsoft.AspNetCore.Http;
using VarzeaLeague.Domain.Interface.Dao;
using VarzeaLeague.Domain.Interface.Utils;
using VarzeaLeague.Domain.Service;
using Moq;
using VarzeaLeague.Domain.Model;
using MongoDB.Driver;
using VarzeaTeam.Domain.Exceptions;

namespace VarzeaLeague.Test.Services;

public class NotificationServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<INotificationDao> _notificationDaoMock;
    private readonly Mock<IHttpContextAccessor> _httpContext;
    private readonly Mock<IGetClientIdToken> _getClientIdToken;
    private readonly NotificationService _notificationServiceMock;

    public NotificationServiceTest()
    {
        _fixture = new Fixture();
        _httpContext = new Mock<IHttpContextAccessor>();
        _notificationDaoMock = new Mock<INotificationDao>();
        _getClientIdToken = new Mock<IGetClientIdToken>();
        _notificationServiceMock = new NotificationService(_notificationDaoMock.Object, _httpContext.Object, _getClientIdToken.Object);
    }

    [Fact]
    public async Task GetAsync_WhenNotificationExist_ReturnsNotificationList()
    {
        // Arrange
        var notificationList = _fixture.CreateMany<NotificationModel>(2).ToList();

        _notificationDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(),
            It.IsAny<FilterDefinition<NotificationModel>>())).ReturnsAsync(notificationList);

        //Act
        var result = await _notificationServiceMock.GetNotificationAsync(1, 10);

        //Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
        Assert.Collection(result,
                item => Assert.Equal(notificationList[0].Id, item.Id),
                item => Assert.Equal(notificationList[1].Id, item.Id)
            );
    }

    [Fact]
    public async Task GetAsync_WhenNoTeamsExist_ThrowsException()
    {
        // Arrange
        _notificationDaoMock.Setup(dao => dao.GetAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<FilterDefinition<NotificationModel>>()))
                   .ReturnsAsync(Enumerable.Empty<NotificationModel>());

        // Act and Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _notificationServiceMock.GetNotificationAsync(1, 10));

        Assert.Equal($"Não existe nenhuma notificação cadastrada", exception.Message);
    }

    [Fact]
    public async Task CreatePlayer_WhenNewPlayer_ReturnsTeam()
    {
        // Arrange
        var notificationModel = _fixture.Create<NotificationModel>();

        _notificationDaoMock.Setup(dao => dao.CreateAsync(notificationModel));

        // Act
        var createdTeam = await _notificationServiceMock.SendNotificationAsync(notificationModel);

        // Assert
        // Verifica se o método CreateAsync foi chamado no DAO com o objeto de time correto
        _notificationDaoMock.Verify(dao => dao.CreateAsync(notificationModel), Times.Once);

        // Verifica se o ClientId foi configurado corretamente no objeto de time criado
        Assert.Equal(notificationModel, createdTeam);
    }

    [Fact]
    public async Task CreatePlayer_WhenNewPlayer_ThrowException()
    {
        // Arrange
        var notificationModel = (NotificationModel)null;

        // Act & Assert
        var exception = await Assert.ThrowsAsync<ExceptionFilter>(async () => await _notificationServiceMock.SendNotificationAsync(notificationModel));

        Assert.Equal($"notificationModel O objeto de notificação não pode ser nulo.", exception.Message);
    }
}
