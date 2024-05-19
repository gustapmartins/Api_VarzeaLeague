using Microsoft.Extensions.Configuration;
using VarzeaLeague.Domain.Interface.Services;
using System.Net.Mail;
using System.Net;

namespace VarzeaLeague.Domain.Service;

public class EmailService: IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendMail(string email, string subject, string message)
    {
        string mail = _configuration["EmailTrap:Email"];

        SmtpClient client = new(_configuration["EmailTrap:Host"], int.Parse(_configuration["EmailTrap:Port"]))
        {
            Credentials = new NetworkCredential(mail, _configuration["EmailTrap:Password"]),
            EnableSsl = true,
            UseDefaultCredentials = false,  
        };


        await client.SendMailAsync(mail, email, subject, message);
    }
}
