using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using VarzeaLeague.Domain.JwtHelper;

namespace VarzeaLeague.Test.Services;

public class GetClientIdTokenTest
{
    private readonly GetClientIdToken _getClientIdToken;
    private readonly string _validSecretKey = "your-256-bit-secret-your-256-bit-secret";

    public GetClientIdTokenTest()
    {
        _getClientIdToken = new GetClientIdToken();
    }

    [Fact]
    public void GetClientIdFromToken_WhenTokenIsPresentAndContainsId_ReturnsClientId()
    {
        // Arrange
        var clientId = "12345";
        var token = GenerateToken(clientId);

        var httpContextMock = new Mock<HttpContext>();
        var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", "Bearer " + token }
            });
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(r => r.Headers).Returns(headers);
        httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

        // Act
        var result = _getClientIdToken.GetClientIdFromToken(httpContextMock.Object);

        // Assert
        Assert.Equal(clientId, result);
    }

    [Fact]
    public void GetClientIdFromToken_WhenTokenIsAbsent_ReturnsNull()
    {
        // Arrange
        var httpContextMock = new Mock<HttpContext>();
        var headers = new HeaderDictionary();
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(r => r.Headers).Returns(headers);
        httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

        // Act
        var result = _getClientIdToken.GetClientIdFromToken(httpContextMock.Object);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void GetClientIdFromToken_WhenTokenDoesNotContainId_ReturnsNull()
    {
        // Arrange
        var token = GenerateTokenWithoutId();

        var httpContextMock = new Mock<HttpContext>();
        var headers = new HeaderDictionary(new Dictionary<string, StringValues>
            {
                { "Authorization", "Bearer " + token }
            });
        var requestMock = new Mock<HttpRequest>();
        requestMock.Setup(r => r.Headers).Returns(headers);
        httpContextMock.Setup(c => c.Request).Returns(requestMock.Object);

        // Act
        var result = _getClientIdToken.GetClientIdFromToken(httpContextMock.Object);

        // Assert
        Assert.Null(result);
    }

    private string GenerateToken(string clientId)
    {
        var claims = new[]
        {
                new Claim("id", clientId)
            };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_validSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private string GenerateTokenWithoutId()
    {
        var claims = new[]
        {
           new Claim("username", "testuser")
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_validSecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
