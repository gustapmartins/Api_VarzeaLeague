using Moq;
using Moq.Protected;
using System.Net;
using VarzeaLeague.Domain.Utils;

namespace VarzeaLeague.Test.Utils
{
    public class ViaCepTest
    {
        //[Fact]
        //public async Task GetCep_ValidCep_ReturnsTrue()
        //{
        //    // Arrange
        //    string cep = "01001000"; // CEP válido para teste
        //    var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        //    mockHandler
        //        .Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //            "SendAsync",
        //            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains(cep)),
        //            ItExpr.IsAny<CancellationToken>()
        //        )
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.OK,
        //            Content = new StringContent("{\"cep\":\"01001-000\",\"logradouro\":\"Praça da Sé\",\"complemento\":\"lado ímpar\",\"bairro\":\"Sé\",\"localidade\":\"São Paulo\",\"uf\":\"SP\",\"ibge\":\"3550308\",\"gia\":\"1004\",\"ddd\":\"11\",\"siafi\":\"7107\"}")
        //        });

        //    HttpClient httpClient = new HttpClient(mockHandler.Object);

        //    // Act
        //    bool result = await ViaCep.GetCep(cep);

        //    // Assert
        //    Assert.True(result);

        //    // Optional: Verify the mock handler was called once
        //    mockHandler.Protected().Verify(
        //        "SendAsync",
        //        Times.Once(),
        //        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains(cep)),
        //        ItExpr.IsAny<CancellationToken>()
        //    );
        //}

        //[Fact]
        //public async Task GetCep_InvalidCep_ReturnsFalse()
        //{
        //    // Arrange
        //    string cep = "99999999"; // CEP inválido para teste
        //    var mockHandler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
        //    mockHandler
        //        .Protected()
        //        .Setup<Task<HttpResponseMessage>>(
        //            "SendAsync",
        //            ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains(cep)),
        //            ItExpr.IsAny<CancellationToken>()
        //        )
        //        .ReturnsAsync(new HttpResponseMessage
        //        {
        //            StatusCode = HttpStatusCode.NotFound
        //        });

        //    HttpClient httpClient = new HttpClient(mockHandler.Object);

        //    // Act
        //    bool result = await ViaCep.GetCep(cep);

        //    // Assert
        //    Assert.False(result);

        //    // Verify the mock handler was called once with the correct request
        //    mockHandler.Protected().Verify(
        //        "SendAsync",
        //        Times.Once(),
        //        ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get && req.RequestUri.ToString().Contains(cep)),
        //        ItExpr.IsAny<CancellationToken>()
        //    );
        //}

    }
}
