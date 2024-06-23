using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VarzeaLeague.Application.Controllers.v1;
using VarzeaLeague.Application.DTO.Notification;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model;

namespace VarzeaLeague.Test.Controllers;

public class NotificationControllerTest
{
    private readonly Fixture _fixture;
    private readonly Mock<INotificationService> _notificationServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly NotificationController _notificationController;

    public NotificationControllerTest()
    {
        _fixture = new Fixture();
        _notificationServiceMock = new Mock<INotificationService>();
        _mapperMock = new Mock<IMapper>();
        _notificationController = new NotificationController(_notificationServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetPlayer_WhenCalled_ReturnsOkResultWithNotifications()
    {
        // Arrange
        var notifications = _fixture.CreateMany<NotificationModel>(2).ToList();
        var notificationViewDtos = _fixture.CreateMany<NotificationViewDto>(2).ToList();

        _notificationServiceMock.Setup(service => service.GetNotificationAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(notifications);

        _mapperMock.Setup(mapper => mapper.Map<List<NotificationViewDto>>(It.IsAny<IEnumerable<NotificationModel>>()))
            .Returns(notificationViewDtos);

        // Act
        var result = await _notificationController.GetPlayer(1, 10) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<List<NotificationViewDto>>(result.Value);
        Assert.Equal(2, returnValue.Count);
    }
}
