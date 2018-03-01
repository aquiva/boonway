using Boonwayy.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Boonwayy.Models
{
    public class UserJoinedProject
    {
        public int Id { get; set; }

        public virtual Project Project { get; set; }
        public int ProjectId { get; set; }

        public virtual ApplicationUser User { get; set; }
        public string ApplicationUserId { get; set; }

    }
}