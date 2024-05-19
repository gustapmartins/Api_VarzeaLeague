using VarzeaLeague.Domain.Interface.Services;
using Microsoft.Extensions.Configuration;
using AutoFixture;
using Ticket.Service;
using Moq;
using VarzeaLeague.Domain.Model;
using Castle.Core.Smtp;
using System.Net.Mail;

namespace VarzeaLeague.Test.Services;

public class EmailServiceTest
{
    private readonly Fixture _fixture;
    private readonly Mock<IConfiguration> _configuration;
    private readonly IEmailService _emailService;

    public EmailServiceTest()
    {
        _fixture = new Fixture();
        _configuration = new Mock<IConfiguration>();
        _emailService = new EmailService(_configuration.Object);
    }

    [Fact]
    public async Task GetAsync_WhenPlayersExist_ReturnsPlayerList()
    {
        //string email = "recipient@example.com";
        //string subject = "Test Subject";
        //string message = "Test Message";

        //var smtpClientMock = _fixture.Create<SmtpClient>();

        //// Act
        //await _emailService.SendMail(email, subject, message);

        //// Assert
        ////_emailService.Verify(
        ////    sender => sender.SendMail(email, subject, message),
        ////    Times.Once
        ////);
    }
}
