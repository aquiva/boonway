using Boonwayy.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace Boonwayy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class DashboardController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            ViewBag.TotalUsers = db.Users.Count();
            ViewBag.TotalProjects = db.Projects.Count();
            ViewBag.TotalProposals = db.UserProposals.Count();

            ViewBag.NewProposals = db.UserProposals.Where(ia => ia.IsApproved == false).Count();

            return View();
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}