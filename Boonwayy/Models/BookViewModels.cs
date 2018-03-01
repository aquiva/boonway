using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models.Book
{
    public class IndexViewModel
    {
        public string StripePublishableKey { get; set; }

        [DataType(DataType.Currency)]
        public int bookPrice { get; set; }
    }

    public class ChargeViewModel
    {
        public string StripeToken { get; set; }
        public string StripeEmail { get; set; }

    }

    public class CustomViewModel
    {
        public string StripePublishableKey { get; set; }

        public string StripeToken { get; set; }
        public string StripeEmail { get; set; }
    }
}