using GomMessage.Application.Auth.Dtos;
using GomMessage.Application.Interfaces;
using GomMessage.Infrastructure.Settings;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace GomMessage.Infrastructure.Services.Email
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> options)
        {
            _mailSettings = options.Value;
        }

        public async Task<bool> SendMail(MailData mailData)
        {
            try
            {
                // MimeMessage - a class from MimeKit
                var message = new MimeMessage();

                message.From.Add(new MailboxAddress(_mailSettings.Name, _mailSettings.EmailId));
                message.To.Add(MailboxAddress.Parse(mailData.EmailToId));

                message.Subject = mailData.EmailSubject;

                var builder = new BodyBuilder();
                if (mailData.IsHtml)
                {
                    builder.HtmlBody = mailData.EmailBody;
                }
                else
                {
                    builder.TextBody = mailData.EmailBody;
                }
                message.Body = builder.ToMessageBody();

                using var smtp = new SmtpClient();
                var socketOption = _mailSettings.Port == 465
                    ? SecureSocketOptions.SslOnConnect
                    : SecureSocketOptions.StartTls;

                await smtp.ConnectAsync(_mailSettings.Host, _mailSettings.Port, socketOption);

                await smtp.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);

                await smtp.SendAsync(message);
                await smtp.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[MailService Error]: {ex.Message}");
                return false;
            }
        }

        public async Task<bool> SendOtpCode(string email, string userName, string otpCode)
        {
            try
            {
                string baseDir = AppContext.BaseDirectory;
                string templatePath = Path.Combine(baseDir, "Services", "Email", "EmailTemplates", "VerifyOtp.html");

                if (!File.Exists(templatePath))
                {
                    Console.WriteLine($"[MailService Error]: File not found at {templatePath}");
                    return false;
                }

                string htmlBody = await File.ReadAllTextAsync(templatePath);
                htmlBody = htmlBody.Replace("{UserName}", userName)
                                   .Replace("{OtpCode}", otpCode);

                var mailData = new MailData
                {
                    EmailToId = email,
                    EmailSubject = "Mã xác thực OTP - GomMessage",
                    EmailBody = htmlBody,
                    IsHtml = true
                };

                return await SendMail(mailData);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[SendOtpCode Error]: {ex.Message}");
                return false;
            }
        }
    }
}