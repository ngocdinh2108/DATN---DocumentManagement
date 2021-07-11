using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace DocumentManagement.Common
{
    public class MailHelper
    {
        public void SendEmail(string toEmailAddress, string subject, string content)
        {
            var fromEmailAddress = ConfigurationManager.AppSettings["FromEmailAddress"].ToString();
            var fromEmailDisplayName = ConfigurationManager.AppSettings["FromEmailDisplayName"].ToString();
            var fromEmailPassword = ConfigurationManager.AppSettings["FromEmailPassword"].ToString();
            var smtpPort = ConfigurationManager.AppSettings["SMTPPort"].ToString();
            var smtpHost = ConfigurationManager.AppSettings["SMTPHost"].ToString();

            bool enabledSSL = bool.Parse(ConfigurationManager.AppSettings["EnabledSSL"].ToString());

            MailMessage mailMessage = new MailMessage(new MailAddress(fromEmailAddress, fromEmailDisplayName), new MailAddress(toEmailAddress));
            mailMessage.Subject = subject;
            mailMessage.Body = content;
            mailMessage.IsBodyHtml = true;

            var client = new SmtpClient();
            client.Credentials = new NetworkCredential(fromEmailAddress, fromEmailPassword);
            client.Host = smtpHost;
            client.Port = !string.IsNullOrEmpty(smtpPort) ? Convert.ToInt32(smtpPort) : 0;
            client.EnableSsl = enabledSSL;
            client.Send(mailMessage);

        }
    }
}