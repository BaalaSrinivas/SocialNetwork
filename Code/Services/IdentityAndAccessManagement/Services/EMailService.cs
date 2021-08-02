using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace IdentityAndAccessManagement.Services
{
    public class EMailService : IEmailService
    {
        public bool SendEmail(string mailId, string subject, string emailBody)
        {
            string fromMail = "";
            string fromPassword = "";

            MailMessage message = new MailMessage();
            message.From = new MailAddress(fromMail);
            message.Subject = subject;
            message.To.Add(new MailAddress(mailId));
            message.Body = "<html><body> " + emailBody + " </body></html>";
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential(fromMail, fromPassword),
                EnableSsl = true,

            };
            smtpClient.Send(message);

            return true;
        }
    }
}
