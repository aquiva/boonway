using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class UserProfile
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }

        [Required]
        [Display(Name = "Age")]
        public int Age { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePictureUrl { get; set; }

        [Display(Name = "Short Bio")]
        public string Bio { get; set; }

        [Required]
        [Display(Name = "Social Security Number")]
        public string SocialSecurityNumber { get; set; }

        [Display(Name = "Your State")]
        public string State { get; set; }

        [Display(Name = "Your Address")]
        public string Address { get; set; }

        [Required]
        [Display(Name = "Your Profession")]
        public string Profession { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ApplicationUserId { get; set; }

    }


    public class UserProfileViewModel
    {
        public HttpPostedFileBase ProfileImageFile { get; set; }

        
    }
}