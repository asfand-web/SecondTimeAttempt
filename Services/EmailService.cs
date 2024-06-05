using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using SecondTimeAttempt.Services;

public interface IEmailService
{
    Task SendEmailAsync(EmailDto emailDto);
    Task SendConfirmationEmailAsync(string to, string confirmationToken);
}

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task SendEmailAsync(EmailDto emailDto)
    {
        var smtpServer = _configuration["EmailSettings:SmtpServer"];
        var smtpPort = int.Parse(_configuration["EmailSettings:SmtpPort"]);
        var senderEmail = _configuration["EmailSettings:SenderEmail"];
        var senderName = _configuration["EmailSettings:SenderName"];
        var username = _configuration["EmailSettings:Username"];
        var password = _configuration["EmailSettings:Password"];

        using (var client = new SmtpClient(smtpServer, smtpPort))
        {
            client.Credentials = new System.Net.NetworkCredential(username, password);
            client.EnableSsl = true; // Enable SSL if required by your email provider

            var mailMessage = new MailMessage
            {
                From = new MailAddress(senderEmail, senderName),
                Subject = emailDto.Subject,
                Body = emailDto.Body,
                IsBodyHtml = true
            };

            mailMessage.To.Add(emailDto.To);

            await client.SendMailAsync(mailMessage);
        }
    }
    public async Task SendConfirmationEmailAsync(string to, string confirmationToken)
    {
        var emailDto = new EmailDto
        {
            To = to,
            Subject = "Email Confirmation",
            Body = $"Please confirm your email by clicking this link: https://localhost:7267/api/auth/confirm-email?token={confirmationToken}"
        };

        await SendEmailAsync(emailDto);
    }
}
