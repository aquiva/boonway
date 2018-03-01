using Stripe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace Boonwayy.Services
{
    public class EmailService
    {
        public void SendRefundEmail(StripeCharge stripeCharge)
        {
            var message = new MailMessage() { IsBodyHtml = true };
            message.To.Add(new MailAddress("safdarrizvi63@gmail.com "));
            message.Subject = string.Format("refund requested on charge {0}", stripeCharge.Id);
            message.Body = string.Format("<p>{0}</p>", string.Format("A customer at this address {0} was issued a refund."), stripeCharge.ReceiptEmail, stripeCharge.Description);

            using (var smtp = new SmtpClient())
            {
                smtp.Send(message);
            }
        }
    }
}