using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        public string UserComment { get; set; }

        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }
    }
}