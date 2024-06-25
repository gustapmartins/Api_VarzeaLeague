using AutoFixture;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Newtonsoft.Json.Linq;
using VarzeaLeague.Application.Controllers.v1;
using VarzeaLeague.Application.DTO.User;
using VarzeaLeague.Domain.Enum;
using VarzeaLeague.Domain.Interface.Services;
using VarzeaLeague.Domain.Model.User;
using VarzeaTeam.Domain.Enum;

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
        // Converta result.Value para JObject para acessar a propriedade 'token'
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
        var resultValue = JObject.Parse(json);

        // Acesse a propriedade 'token' diretamente
        var tokenValue = resultValue["token"].ToString();
        Assert.Equal(token, tokenValue);
    }

    [Fact]
    public async Task PostTeam_WhenCalled_ReturnsCreatedAtActionResult()
    {
        // Arrange
        var fixture = new Fixture();

        // Configure the fixture to create valid instances of RegisterDto
        fixture.Customize<RegisterDto>(composer => composer
            .With(r => r.Username, "ValidUsername")
            .With(r => r.Email, "valid@example.com")
            .With(r => r.Password, "Valid1234")
            .With(r => r.ConfirmPassword, "Valid1234")
            .With(r => r.Cpf, "123.456.789-09")
            .With(r => r.Role, Role.Admin)); // Adjust Role to a valid enum value

        var registerDto = fixture.Create<RegisterDto>();
        var userModel = fixture.Create<UserModel>();

        _mapperMock.Setup(mapper => mapper.Map<UserModel>(It.IsAny<RegisterDto>()))
            .Returns(userModel);

        // Configure the mock to return a Task<UserModel>
        _authServiceMock.Setup(service => service.CreateAsync(It.IsAny<UserModel>()))
            .ReturnsAsync(userModel);

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

        // Converta result.Value para JObject para acessar a propriedade 'token'
        var json = Newtonsoft.Json.JsonConvert.SerializeObject(result.Value);
        var resultValue = JObject.Parse(json);

        // Acesse a propriedade 'token' diretamente
        var tokenValue = resultValue["token"].ToString();
        Assert.Equal(token, tokenValue);
    }

    [Fact]
    public async Task ResetPassword_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        var passwordResetDto = _fixture.Create<PasswordResetDto>();
        var passwordReset = _fixture.Create<PasswordReset>();

        _mapperMock.Setup(mapper => mapper.Map<PasswordReset>(It.IsAny<PasswordResetDto>()))
            .Returns(passwordReset);

        // Ajuste na configuração do mock para o método ResetPassword
        _authServiceMock.Setup(service => service.ResetPassword(It.IsAny<PasswordReset>()))
            .ReturnsAsync("Senha redefinida"); // Exemplo de retorno do método ResetPassword

        // Act
        var result = await _authController.ResetPassword(passwordResetDto) as OkObjectResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(StatusCodes.Status200OK, result.StatusCode);
        Assert.Equal("Senha redefinida", result.Value); // Verifica se o valor retornado é o esperado
    }

    [Fact]
    public async Task RemoveUser_WhenCalled_ReturnsOkResult()
    {
        // Arrange
        UserModel userMock = _fixture.Build<UserModel>()
                               .With(x => x.AccountStatus, AccountStatus.active)
                               .Create();

        _authServiceMock.Setup(service => service.RemoveAsync(It.IsAny<string>()))
            .ReturnsAsync(userMock);

        // Act
        var result = await _authController.RemoveUser(It.IsAny<string>()) as OkObjectResult;

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
