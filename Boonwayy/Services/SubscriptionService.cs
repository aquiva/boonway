using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Stripe;
using Boonwayy;
using Boonwayy.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;


namespace Boonwayy.Services
{
    public class SubscriptionService : ISubscriptionService
    {
        private ApplicationUserManager userManager;
        public ApplicationUserManager UserManager
        {
            get
            {
                return userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                userManager = value;
            }
        }

        private StripeCustomerService customerService;
        public StripeCustomerService StripeCustomerService
        {
            get
            {
                return customerService ?? new StripeCustomerService();
            }
            private set
            {
                customerService = value;
            }
        }

        private StripeSubscriptionService subscriptionService;
        public StripeSubscriptionService StripeSubscriptionService
        {
            get
            {
                return subscriptionService ?? new StripeSubscriptionService();
            }
            private set
            {
                subscriptionService = value;
            }
        }

        public SubscriptionService()
        {

        }

        public SubscriptionService(ApplicationUserManager userManager, StripeCustomerService customerService, StripeSubscriptionService subscriptionService)
        {
            this.userManager = userManager;
            this.customerService = customerService;
            this.subscriptionService = subscriptionService;
        }

        public void Create(string userName, Plan plan, string stripeToken)
        {
            var user = UserManager.FindByName(userName);

            if (String.IsNullOrEmpty(user.StripeCustomerId))
            {
                //create customer
                var customer = new StripeCustomerCreateOptions()
                {
                    Email = user.Email,
                    SourceToken = stripeToken,
                    PlanId = plan.ExternalId

                };

                StripeCustomer stripeCustomer = StripeCustomerService.Create(customer);

                user.StripeCustomerId = stripeCustomer.Id;
                user.ActiveUntil = DateTime.Now.AddDays(30);
                UserManager.Update(user);
                
            }
            else
            {
                //update customer
                var stripeSubscription = StripeSubscriptionService.Create(user.StripeCustomerId, plan.ExternalId);
                user.ActiveUntil = DateTime.Now.AddDays(30);
                UserManager.Update(user);
            }
        }

    }
}