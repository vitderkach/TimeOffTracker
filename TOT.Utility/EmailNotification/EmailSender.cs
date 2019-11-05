using Microsoft.AspNetCore.Identity.UI.Services;
using System;
using MimeKit;
using MimeKit.Text;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using TOT.Interfaces;

namespace TOT.Utility.EmailNotification
{
    public class EmailSender : IEmailSender
    {
        private readonly IEmailSendConfiguration emailConfiguration;

        public EmailSender(IEmailSendConfiguration emailconfig)
        {
            emailConfiguration = emailconfig;
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MimeMessage message = new MimeMessage();

            try
            {
                message.From.Add(new MailboxAddress("(no-reply) Time off tracker", emailConfiguration.SmtpUsername));
                message.To.Add(new MailboxAddress(email, email));
                message.Subject = subject;

                message.Body = new TextPart(TextFormat.Html)
                {
                    Text = htmlMessage
                };

                using (var emailClient = new SmtpClient())
                {
                    //emailClient.AuthenticationMechanisms.Remove("XOAUTH2");
                    await emailClient.ConnectAsync(emailConfiguration.SmtpServer, emailConfiguration.SmtpPort, false);                    
                    await emailClient.AuthenticateAsync(emailConfiguration.SmtpUsername, emailConfiguration.SmtpPassword);

                    await emailClient.SendAsync(message);
                    await emailClient.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new NotImplementedException("An error occurred while sending the Email" + ex.Message);
            }
        }
    }
}
