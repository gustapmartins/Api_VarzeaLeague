
using AutoFixture;
using Microsoft.Extensions.Configuration;
using Moq;
using Ticket.Service;

namespace VarzeaLeague.Test.Services;

public class EmailServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IConfiguration> _configuration;
    private readonly EmailService _emailService;

    public EmailServiceTest()
    {
        _fixture = new Fixture();
        _configuration = new Mock<IConfiguration>();
        _emailService = new EmailService(_configuration.Object);
    }
}
