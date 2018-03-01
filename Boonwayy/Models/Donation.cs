using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class Donation
    {
        public int Id { get; set; }

        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        [Display(Name = "Full Name")]
        [Required]
        public string DonatorName { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string City { get; set; }

        public string State { get; set; }

        [Display(Name = "Donation Amount")]
        [DataType(DataType.Currency)]
        [Range(5, 2999)]
        [Required]
        public int AmountInCents { get; set; }

        //[DataType(DataType.Currency)]
        //public int AmountInDollars
        //{
        //    get
        //    {
        //        return (int)Math.Floor((decimal)this.AmountInCents / 100);
        //    }
        //}

        public DateTime DonationDate { get; set; }

    }
}