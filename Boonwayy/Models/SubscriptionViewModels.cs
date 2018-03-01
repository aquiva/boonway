using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Boonwayy.Data;

namespace Boonwayy.Models.Subscription
{
    public class IndexViewModel
    {
        public IList<Plan> Plans { get; internal set; }
    }

    public class BillingViewModel
    {
        //public int planId { get; set; }

        public Plan Plan { get; set; }

        public string StripePublishableKey { get; set; }

        public string StripeToken { get; set; }
    }
}