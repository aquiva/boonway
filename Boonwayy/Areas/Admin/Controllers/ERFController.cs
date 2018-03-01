using Boonwayy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boonwayy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ERFController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/ERF
        public ActionResult Index()
        {
            var model = db.ERFDonations.ToList();

            return View(model);
        }

        public ActionResult MemberContributions()
        {
            var model = db.MemberContributions.ToList();

            return View(model);
        }
    }
}