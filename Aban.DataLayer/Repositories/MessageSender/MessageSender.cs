using Aban.DataLayer.Interfaces.MessageSender;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Aban.DataLayer.Repositories.MessageSender
{
    public class MessageSender : IMessageSender
    {
        private readonly IOptions<NetworkCredential> _networkCredential;

        public MessageSender(
            IOptions<NetworkCredential> networkCredential)
        {
            _networkCredential = networkCredential;
        }

        public Task SendEmailAsync(
            string toEmail,
            string subject,
            string message,
            bool isMessageHtml = false)
        {
            using (SmtpClient client = new SmtpClient())
            {
                var credentials = new NetworkCredential()
                {
                    UserName = _networkCredential.Value.UserName, //whithout @gmail.com
                    Password = _networkCredential.Value.Password
                };

                client.Credentials = credentials;
                client.Host = "smtp.gmail.com";
                client.Port = 587;
                client.EnableSsl = true;

                using MailMessage emailMessage = new MailMessage()
                {
                    To = { new MailAddress(toEmail) },
                    From = new MailAddress(""), //whti @gmail.com
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = isMessageHtml
                };

                client.Send(emailMessage);
            }

            return Task.CompletedTask;
        }
    }
}