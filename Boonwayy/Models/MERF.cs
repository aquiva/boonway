using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class MERF
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Story { get; set; }

        public string VideoUrl { get; set; }

        [Required]
        [Display(Name = "Start Date")]
        public DateTime MERFStartDate { get; set; }

        public virtual ApplicationUser User { get; set; }

        public string ApplicationUserId { get; set; }
    }
}