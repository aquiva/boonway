using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class MySubscriptionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Interval { get; set; }
        [DataType(DataType.Currency)]
        public int Amount { get; set; }
        public string Desc { get; set; }
        public DateTime Created { get; set; }
        public DateTime Renewal { get; set; }
    }
}