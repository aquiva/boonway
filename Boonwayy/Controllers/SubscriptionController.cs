using Boonwayy.Models.Subscription;
using Boonwayy.Services;
using Microsoft.AspNet.Identity;
using Stripe;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boonwayy.Controllers
{
    [Authorize]
    public class SubscriptionController : Controller
    {
        private Models.ApplicationDbContext db = new Models.ApplicationDbContext();

        private IPlanService planService;

        private ISubscriptionService subscriptionService;

        public ISubscriptionService SubscriptionService
        {
            get
            {
                return subscriptionService ?? new SubscriptionService();
            }
            private set
            {
                subscriptionService = value;
            }
        }

        public IPlanService PlanService
        {
            get
            {
                return planService ?? new PlanService();
            }
            private set
            {
                planService = value;
            }
        }

        public SubscriptionController(IPlanService planService, ISubscriptionService subscriptionService)
        {
            this.planService = planService;
            this.subscriptionService = subscriptionService;
        }

        public SubscriptionController()
        {

        }

        

        // GET: Subscription
        public ActionResult Index()
        {
            //var userId = User.Identity.GetUserId();
            //var userInformation = db.UserProposals.Where(ui => ui.ApplicationUserId == userId).FirstOrDefault();
            var viewModel = new IndexViewModel() { Plans = PlanService.List() };
            //try
            //{
            //    if (userInformation.IsSubmitted == true && userInformation.IsApproved == true)
            //    {
            //        return View(viewModel);
            //    }
            //    else
            //    {
            //        return RedirectToAction("CreateUserProposal", "Manage");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if (ex.Message == "Object reference not set to an instance of an object.")
            //    {
            //        return RedirectToAction("CreateUserProposal", "Manage");
            //    }

            //}

            
            return View(viewModel);
        }

        public ActionResult Billing(int id)//int planId
        {

            var userId = User.Identity.GetUserId();
            var userInformation = db.UserProposals.Where(ui => ui.ApplicationUserId == userId).FirstOrDefault();
            //var viewModel = new IndexViewModel() { Plans = PlanService.List() };
            try
            {
                if (userInformation.IsSubmitted == true && userInformation.IsApproved == true)
                {
                    var plan = PlanService.Find(id);

                    string stripePublishableKey = ConfigurationManager.AppSettings["stripePublishableKey"];
                    //var viewModel = new BillingViewModel() { Plan = PlanService.Find(planId), StripePublishableKey = stripePublishableKey };

                    var model = new BillingViewModel { Plan = plan, StripePublishableKey = stripePublishableKey };

                    //return RedirectToAction("Billing", "Subscription", new { PlanId = model.Plan.Id, stripePublishableKey });

                    return View(model);
                }
                else
                {
                    return RedirectToAction("CreateUserProposal", "Manage");
                }
            }
            catch (Exception ex)
            {
                if (ex.Message == "Object reference not set to an instance of an object.")
                {
                    return RedirectToAction("CreateUserProposal", "Manage");
                }

            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Billing(BillingViewModel billingViewModel)
        {


            billingViewModel.Plan = PlanService.Find(billingViewModel.Plan.Id);

            try
            {
                SubscriptionService.Create(User.Identity.Name, billingViewModel.Plan, billingViewModel.StripeToken);
            }
            catch (StripeException stripeException)
            {
                ModelState.AddModelError(string.Empty, stripeException.Message);
                return View(billingViewModel);
            }

            return RedirectToAction("Index", "Manage");
        }
    }
}