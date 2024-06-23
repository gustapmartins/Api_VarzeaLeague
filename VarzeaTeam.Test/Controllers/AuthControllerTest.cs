using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using VarzeaLeague.Application.Controllers.v1;
using VarzeaLeague.Application.DTO.User;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model.User;

namespace VarzeaLeague.Test.Controllers;

public class AuthControllerTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IAuthService> _authServiceMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthController _authController;

    public AuthControllerTest()
    {
        _fixture = new Fixture();
        _authServiceMock = new Mock<IAuthService>();
        _mapperMock = new Mock<IMapper>();
        _authController = new AuthController(_authServiceMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetUsers_WhenCalled_ReturnsOkResultWithUsers()
    {
        // Arrange
        var users = _fixture.CreateMany<UserModel>(2).ToList();
        var userViewDtos = _fixture.CreateMany<UserViewDto>(2).ToList();

        _authServiceMock.Setup(service => service.GetAsync(It.IsAny<int>(), It.IsAny<int>()))
            .ReturnsAsync(users);

        _mapperMock.Setup(mapper => mapper.Map<List<UserViewDto>>(It.IsAny<IEnumerable<UserModel>>()))
            .Returns(userViewDtos);

        // Act
        var result = await _authController.GetUsers(1, 10) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<List<UserViewDto>>(result.Value);
        Assert.Equal(2, returnValue.Count);
    }

    [Fact]
    public async Task GetUserId_WhenCalled_ReturnsOkResultWithUser()
    {
        // Arrange
        var user = _fixture.Create<UserModel>();
        var userViewDto = _fixture.Create<UserViewDto>();

        _authServiceMock.Setup(service => service.GetIdAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        _mapperMock.Setup(mapper => mapper.Map<UserViewDto>(It.IsAny<UserModel>()))
            .Returns(userViewDto);

        // Act
        var result = await _authController.GetUserId("1") as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<UserViewDto>(result.Value);
        Assert.Equal(userViewDto, returnValue);
    }

    [Fact]
    public async Task LoginAsync_WhenCalled_ReturnsOkResultWithToken()
    {
        // Arrange
        var loginDto = _fixture.Create<LoginDto>();
        var userModel = _fixture.Create<UserModel>();
        var token = _fixture.Create<string>();

        _mapperMock.Setup(mapper => mapper.Map<UserModel>(It.IsAny<LoginDto>()))
            .Returns(userModel);

        _authServiceMock.Setup(service => service.Login(It.IsAny<UserModel>()))
            .ReturnsAsync(token);

        // Act
        var result = await _authController.LoginAsync(loginDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<dynamic>(result.Value);
        Assert.Equal(token, returnValue.token);
    }

    [Fact]
    public async Task PostTeam_WhenCalled_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var registerDto = _fixture.Create<RegisterDto>();
        var userModel = _fixture.Create<UserModel>();

        _mapperMock.Setup(mapper => mapper.Map<UserModel>(It.IsAny<RegisterDto>()))
            .Returns(userModel);

        _authServiceMock.Setup(service => service.CreateAsync(It.IsAny<UserModel>()))
            .Returns((Task<UserModel>)Task.CompletedTask);

        // Act
        var result = await _authController.PostTeam(registerDto) as CreatedAtActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status201Created, result.StatusCode);
    }

    [Fact]
    public async Task ForgetPassword_WhenCalled_ReturnsOkResultWithToken()
    {
        // Arrange
        var email = _fixture.Create<string>();
        var token = _fixture.Create<string>();

        _authServiceMock.Setup(service => service.ForgetPassword(It.IsAny<string>()))
            .ReturnsAsync(token);

        // Act
        var result = await _authController.ForgetPassword(email) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<dynamic>(result.Value);
        Assert.Equal(token, returnValue.token);
    }

    [Fact]
    public async Task ResetPassword_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var passwordResetDto = _fixture.Create<PasswordResetDto>();
        var passwordReset = _fixture.Create<PasswordReset>();

        _mapperMock.Setup(mapper => mapper.Map<PasswordReset>(It.IsAny<PasswordResetDto>()))
            .Returns(passwordReset);

        _authServiceMock.Setup(service => service.ResetPassword(It.IsAny<PasswordReset>()))
            .Returns((Task<string>)Task.CompletedTask);

        // Act
        var result = await _authController.ResetPassword(passwordResetDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task RemoveUser_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var userId = _fixture.Create<string>();

        _authServiceMock.Setup(service => service.RemoveAsync(It.IsAny<string>()))
            .Returns((Task<UserModel>)Task.CompletedTask);

        // Act
        var result = await _authController.RemoveUser(userId) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
    }

    [Fact]
    public async Task UpdateMatch_WhenCalled_ReturnsOkResultWithUser()
    {
        // Arrange
        var userUpdateDto = _fixture.Create<UserUpdateDto>();
        var userModel = _fixture.Create<UserModel>();
        var userViewDto = _fixture.Create<UserViewDto>();

        _mapperMock.Setup(mapper => mapper.Map<UserModel>(It.IsAny<UserUpdateDto>()))
            .Returns(userModel);

        _authServiceMock.Setup(service => service.UpdateAsync(It.IsAny<string>(), It.IsAny<UserModel>()))
            .ReturnsAsync(userModel);

        _mapperMock.Setup(mapper => mapper.Map<UserViewDto>(It.IsAny<UserModel>()))
            .Returns(userViewDto);

        // Act
        var result = await _authController.UpdateMatch("1", userUpdateDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        var returnValue = Assert.IsType<UserViewDto>(result.Value);
        Assert.Equal(userViewDto, returnValue);
    }
}
