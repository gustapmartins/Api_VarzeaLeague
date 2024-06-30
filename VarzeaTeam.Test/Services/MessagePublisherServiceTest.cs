using VarzeaLeague.Domain.Service;
using System.Reflection;
using Confluent.Kafka;
using Moq;

namespace VarzeaLeague.Test.Services;

public class MessagePublisherServiceTest
{
    private readonly Mock<IProducer<string, string>> _mockProducer;
    private readonly ProducerConfig _config;
    private readonly MessagePublisherService _messagePublisherService;

    public MessagePublisherServiceTest()
    {
        _mockProducer = new Mock<IProducer<string, string>>();
        _config = new ProducerConfig
        {
            BootstrapServers = "localhost:9092" // Substitua pelo seu servidor Kafka local ou de teste
        };
        _messagePublisherService = new MessagePublisherService(_config.BootstrapServers);
    }

    [Fact]
    public async Task ProduceAsync_ProducesMessageSuccessfully()
    {
        // Arrange
        string message = "Test message";

        _mockProducer
            .Setup(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string t, Message<string, string> msg, CancellationToken token) =>
            {
               
                return new DeliveryResult<string, string>
                {
                    Message = msg,
                    Offset = 1 // Simula um offset qualquer para o teste
                };
            });

        // Substitui o produtor real pelo mock
        var producerField = typeof(MessagePublisherService)
            .GetField("_producer", BindingFlags.Instance | BindingFlags.NonPublic);
        producerField.SetValue(_messagePublisherService, _mockProducer.Object);


        // Act
        await _messagePublisherService.ProduceAsync(message);

        _mockProducer.Verify(p => p.ProduceAsync(It.IsAny<string>(), It.IsAny<Message<string, string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}
