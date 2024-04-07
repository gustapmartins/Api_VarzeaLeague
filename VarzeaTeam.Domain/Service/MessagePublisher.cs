using VarzeaLeague.Domain.Interface.Services;
using VarzeaTeam.Domain.Common;
using Confluent.Kafka;

namespace VarzeaLeague.Domain.Service;

public class MessagePublisher : IMessagePublisher
{
    private readonly ProducerConfig _config;

    public MessagePublisher(string bootstrapServers) 
    {
        _config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers,  
        };
    }

    public async Task ProduceAsync(string message)
    {
        var producer = new ProducerBuilder<Null, string>(_config).Build();

        try
        {
            var deliveryResult = await producer.ProduceAsync(Constans.KAFKA_TOPICO_VARZEALEAGUE, new Message<Null, string> { Value = message });

            Console.WriteLine($"Mensagem produzida: '{message}', Offset: {deliveryResult.Offset}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao produzir mensagem: {ex.Message}");
        }
    }
}
