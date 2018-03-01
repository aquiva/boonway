using Boonwayy.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Boonwayy.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserProposalsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Manager/UserProposals
        public ActionResult Index()
        {
            ViewBag.ApprovedApplications = db.UserProposals.Where(ia => ia.IsApproved == true).Count();

            var model = db.UserProposals.Where(ia => ia.IsApproved == false).ToList();

            return View(model);
        }

        public ActionResult Approve(int id)
        {
            //get entity by id
            var entity = db.UserProposals.Find(id);
            //attach with current values and set new values
            db.Entry(entity).CurrentValues.SetValues(entity.IsApproved = true);
            //save changes in databases
            db.SaveChanges();


            return RedirectToAction("Index");
        }

        public ActionResult Unapprove(int id)
        {
            //get entity by id
            var entity = db.UserProposals.Find(id);
            //attach with current values and set new values
            db.Entry(entity).CurrentValues.SetValues(entity.IsApproved = false);
            //save changes in databases
            db.SaveChanges();

            return RedirectToAction("Approved");
        }


        public ActionResult Approved()
        {
            ViewBag.UnapprovedApplications = db.UserProposals.Where(ia => ia.IsApproved == false).Count();

            var model = db.UserProposals.Where(ia => ia.IsApproved == true).ToList();

            return View(model);
        }

        public ActionResult Delete(int id)
        {
            var entity = db.UserProposals.Find(id);
            db.UserProposals.Attach(entity);
            db.UserProposals.Remove(entity);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
    }
}