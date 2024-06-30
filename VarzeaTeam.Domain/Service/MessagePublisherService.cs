using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Domain.Common;
using Confluent.Kafka;

namespace VarzeaLeague.Domain.Service;

public class MessagePublisherService : IMessagePublisher
{
    private readonly ProducerConfig _config;
    private readonly IProducer<string, string> _producer;

    public MessagePublisherService(string bootstrapServers) 
    {
        _config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,
        };

        _producer = new ProducerBuilder<string, string>(_config).Build();

    }

    public async Task ProduceAsync(string message)
    {        
        try
        {
            var key = "chave-para-a-mensagem";
            
            var deliveryResult = await _producer.ProduceAsync(Constants.KAFKA_TOPICO_VARZEALEAGUE, new Message<string, string> { Key = key, Value = message });

            Console.WriteLine($"Mensagem produzida: '{message}', Offset: {deliveryResult.Offset}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao produzir mensagem: {ex.Message}");
        }
    }
}
