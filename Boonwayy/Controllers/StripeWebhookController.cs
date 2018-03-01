using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;
using Stripe;

namespace Boonwayy.Controllers
{
    public class StripeWebhookController : Controller
    {
        [HttpPost]
        public ActionResult Index()
        {
            Stream request = Request.InputStream;
            request.Seek(0, SeekOrigin.Begin);
            string json = new StreamReader(request).ReadToEnd();
            StripeEvent stripeEvent = null;

            try
            {
                stripeEvent = StripeEventUtility.ParseEvent(json);

            }
            catch (Exception ex)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, string.Format("Unable to parse incoming event. The following error occurred: {0}", ex.Message));
            }

            if (stripeEvent == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Incoming Event Empty!");

            var emailService = new Boonwayy.Services.EmailService();

            switch(stripeEvent.Type)
            {
                case StripeEvents.ChargeRefunded:
                    var charge = Mapper<StripeCharge>.MapFromJson(stripeEvent.Data.Object.ToString());
                    emailService.SendRefundEmail(charge);
                    break;

                default:
                    break;
            }


            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }
    }
}