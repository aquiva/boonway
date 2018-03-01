using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Configuration;

namespace Boonwayy.Models
{
    public class MessageService
    {
        public async static Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                var _email = ConfigurationManager.AppSettings["Email"]; ;
                var _password = ConfigurationManager.AppSettings["Password"];

                var _DispName = "Boonwayy";

                MailMessage myMessage = new MailMessage();

                myMessage.To.Add(email);
                myMessage.From = new MailAddress(_email, _DispName);
                myMessage.Subject = subject;
                myMessage.Body = message;
                myMessage.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.EnableSsl = true;
                    smtp.Host = "mail.boonway.org";
                    smtp.Port = 465;
                    smtp.UseDefaultCredentials = false;
                    smtp.Credentials = new NetworkCredential(_email, _password);
                    smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                    smtp.SendCompleted += (s, e) => { smtp.Dispose(); };
                    await smtp.SendMailAsync(myMessage);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}