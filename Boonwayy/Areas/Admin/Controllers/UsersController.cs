using Boonwayy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boonwayy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        // GET: Manager/Users
        public ActionResult Index()
        {
            var data = (from ue in db.Users
                        where ue.Email != "admin@boonwayy.com"
                        select ue).ToList();

            return View(data);
        }

        public ActionResult UserProfile(string id)
        {
            var profile = db.UserProfiles.Where(aid => aid.ApplicationUserId == id).FirstOrDefault();

            Session["userId"] = id;
            Session["userName"] = profile.FullName;

            return View(profile);
        }

        public ActionResult UserProjects(string id)
        {
            var projects = db.Projects.Where(aid => aid.ApplicationUserId == id).ToList();
            var profile = db.UserProfiles.Where(aid => aid.ApplicationUserId == id).FirstOrDefault();

            

            ViewBag.UserFullName = profile.FullName;

            return View(projects);
        }

        public ActionResult ProjectDonations(int id)
        {
            var donations = db.Donations.Where(pid => pid.ProjectId == id).ToList();

            var project = db.Projects.Where(pid => pid.Id == id).FirstOrDefault();

            ViewBag.ProjectTitle = project.ProjectTitle;

            return View(donations);
        }

        public ActionResult DeleteProject(int id)
        {
            var project = db.Projects.Find(id);

            db.Projects.Attach(project);
            db.Projects.Remove(project);
            db.SaveChanges();

            var userId = (string)Session["userId"];

            return RedirectToAction("UserProjects", new { id = userId });
        }

        public ActionResult UserERFs(string id)
        {
            var userErfs = db.MERFs.Where(aid => aid.ApplicationUserId == id).ToList();

            return View(userErfs);
        }

        public ActionResult DeleteERF(int id)
        {
            var erf = db.MERFs.Find(id);

            db.MERFs.Attach(erf);
            db.MERFs.Remove(erf);
            db.SaveChanges();

            var userId = (string)Session["userId"];

            return RedirectToAction("UserERFs", new { id = userId });
        }
    }
}