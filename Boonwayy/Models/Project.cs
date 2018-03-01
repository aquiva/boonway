using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project Title")]
        public string ProjectTitle { get; set; }

        [Display(Name = "Project Cover Image")]
        public string ProjectCoverImgUrl { get; set; }

        [Required]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Country")]
        public string Country { get; set; }

        [Display(Name = "List Main Needs")]
        public string ProjectNeeds { get; set; }

        [Required]
        [DataType(DataType.Currency)]
        [Range(500, 50000)]
        [Display(Name = "Budget Goal (USD)")]
        public decimal BudgetGoal { get; set; }

        
        [DataType(DataType.Currency)]
        [Display(Name = "Amount Contributed")]
        public decimal AmountContributed { get; set; }

        [Required]
        [Display(Name = "Project Niche")]
        public string ProjectNiche { get; set; }

        [Display(Name = "Project Category")]
        public string ProjectCategory { get; set; }

        [Required]
        [Display(Name = "Project Description")]
        public string ProjectDescription { get; set; }

        [Display(Name = "Youtube Video Link")]
        public string YoutubeVideoUrl { get; set; }

        [Required]
        [Display(Name = "Project Star tDate")]
        public DateTime ProjectStartDate { get; set; }

        [Required]
        [Display(Name = "Project End Date")]
        public DateTime ProjectEndDate { get; set; }

        [Display(Name = "Project Status")]
        public bool ProjectStatus { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ApplicationUserId { get; set; }

        //public virtual ICollection<UserProfile> UserProfiles { get; set; }
    }

    public class ProjectViewModel
    {
        public HttpPostedFileBase CoverImageFile { get; set; }
        
        public bool postInCW { get; set; }
    }
}