using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class MemberContribution
    {
        //The class is related to member contributions in ERF

        public int Id { get; set; }

        [Display(Name = "Donation Amount")]
        [DataType(DataType.Currency)]
        [Range(25, 300)]
        [Required]
        public int AmountInCents { get; set; }

        //public virtual MERF MERF { get; set; }
        //public int MERFID { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }

        public DateTime ContributionDate { get; set; }
    }
}