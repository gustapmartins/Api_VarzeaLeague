namespace VarzeaLeague.Domain.Interface.Services;

public interface IMessagePublisher
{
    Task ProduceAsync(string message);
}
