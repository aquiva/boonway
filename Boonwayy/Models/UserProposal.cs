using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class UserProposal
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Social Security #")]
        public string SocialSecurityNumber { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Address")]
        public string Address { get; set; }

        [Display(Name = "Offer Letter")]
        public string OfferLetterUrl { get; set; }

        [Display(Name = "Driving License")]
        public string DriverLicenseUrl { get; set; }

        public bool IsApproved { get; set; }

        public bool IsSubmitted { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ApplicationUserId { get; set; }
    }

    public class UserProposalViewModel
    {
        public HttpPostedFileBase offerLetterFile { get; set; }

        public HttpPostedFileBase driverLicenseFile { get; set; }
    }
}