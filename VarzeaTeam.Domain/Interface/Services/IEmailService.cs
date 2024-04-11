namespace VarzeaLeague.Domain.Interface.Services;

public interface IEmailService
{
    void SendMail(string email, string subject, string message);
}
