namespace VarzeaLeague.Domain.Interface.Services;

public interface IEmailService
{
    Task SendMail(string from, string email, string subject, string message);
}
