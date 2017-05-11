using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace CodeKingdom.Notify
{
    public class Mailer
    {
        private SmtpClient client;
        private string password;

        private MailMessage mail;

        public Mailer(string to, string subject, string message)
        {
            password = ConfigurationManager.AppSettings["FromEmailPassword"];
            mail = new MailMessage();
            mail.From = new MailAddress(ConfigurationManager.AppSettings["FromEmailAddress"]);
            mail.To.Add(new MailAddress(to));
            mail.Body = message;
            mail.Subject = subject;
            


            client = new SmtpClient();
            client.Host = "smtp.gmail.com";
            client.Port = 587;
            client.UseDefaultCredentials = false;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.Credentials = new NetworkCredential(mail.From.Address, password);
            client.EnableSsl = true;
        }

        public void Send()
        {
            client.Send(mail);
        }
    }
}