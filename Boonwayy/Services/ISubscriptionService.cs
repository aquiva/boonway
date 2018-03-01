using Boonwayy.Data;
using Stripe;

namespace Boonwayy.Services
{
    public interface ISubscriptionService
    {
        StripeCustomerService StripeCustomerService { get; }
        StripeSubscriptionService StripeSubscriptionService { get; }
        ApplicationUserManager UserManager { get; }

        void Create(string userName, Plan plan, string stripeToken);
    }
}