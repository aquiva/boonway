using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Boonwayy.Models;
using Microsoft.AspNet.Identity;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.Owin;
using Stripe;
using System.Configuration;

namespace Boonwayy.Controllers
{
    public class HomeController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        public ActionResult Index()
        {
            return View();
        }

        

        [Authorize]
        public ActionResult Profiles(string id)
        {
            var model = db.UserProfiles.Where(aid => aid.ApplicationUserId == id).ToList();

            return PartialView(model);
        }

        [Authorize]
        public ActionResult UserProjects()
        {
            var model = (from p in db.Projects
                         orderby p.Id descending
                         select p).ToList();

            return PartialView(model);
        }


        [Authorize]
        [HttpPost]
        public ActionResult Search(FormCollection formCollection)
        {
            var search = formCollection["txtSearchTitle"];

            ViewBag.SearchResultFor = search;

            var model = db.Projects.Where(pt => pt.ProjectTitle.Contains(search)).ToList();

            return View(model);
        }

        public ActionResult Terms()
        {
            return View();
        }

        public ActionResult Privacy()
        {
            return View();
        }

        
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            //ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpPost]
        public ActionResult Contact(FormCollection formCollection)
        {
            //return "Contact Form Data: \n" + formCollection["txtFullName"] + "\n" + formCollection["txtEmail"] + "\n" + formCollection["txtSubject"] + "\n" + formCollection["txtMessage"];

            using(MailMessage mm = new MailMessage(formCollection["txtEmail"], "postmaster@boonway.org"))
            {
                mm.Subject = "Contact";
                mm.Body = "<p>Name: " + formCollection["txtFullName"] + "</p>"
                    + "<p>Subject: " + formCollection["txtSubject"] + "</p>"
                    + "<p>Message: " + formCollection["txtMessage"] + "</p>";
                mm.IsBodyHtml = true;
                
                using(SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "mail.boonway.org";
                    smtp.EnableSsl = false;
                    NetworkCredential nc = new NetworkCredential("postmaster@boonway.org", "smartway11@");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Port = 8889;
                    smtp.Send(mm);

                    ViewBag.Message = "Thanks you for contacting us.";

                    return View();
                }

            }

            //return "Mail was not sent";

            //return View();
        }

        [Authorize]
        public ActionResult Explore()
        {
            var model = db.Projects.ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult Project(int id)
        {
            var userProjectById = db.Projects.Find(id);

            var userId = User.Identity.GetUserId();

            Session["projectHolderId"] = userProjectById.ApplicationUserId;
            Session["loggedInUserId"] = userId;
            Session["projectId"] = id;

            var userProfile = db.UserProfiles.Where(aid => aid.ApplicationUserId == userProjectById.ApplicationUserId).FirstOrDefault();

            //------------------------User Votes

            //get if a user has voted a project
            var UV = (from v in db.ProjectVotes
                      where v.ApplicationUserId == userId && v.ProjectId == id
                      select v);

            //count a user vote if it is 1 then user will be able to unvote the project
            ViewBag.UserHasVote = UV.Count();

            //if(ViewBag.UserHasVote == 1)
            //{
            //    ViewBag.UserHasVote = "Vote Down";
            //}

            //get total votes on project by project id
            var totalVotes = (from v in db.ProjectVotes
                              where v.ProjectId == id
                              select v);

            ViewBag.totalVotes = totalVotes.Count();

            //------------------------User Votes END


            //------------------------User Likes

            //get if a user has liked the project
            var UL = (from e in db.ProjectLikes
                      where e.ApplicationUserId == userId && e.ProjectId == id
                      select e);

            //counts a user like if it is 1 then user will be able to unlike the project
            ViewBag.UserHasLike = UL.Count();
            //if (ViewBag.UserHasLike == 1)
            //{
            //    ViewBag.LikeMessage = "Unlike Project";
            //}

            //get total project likes
            var LikeCount = (from e in db.ProjectLikes
                             where e.ProjectId == id
                             select e);

            ViewBag.LikeCount = LikeCount.Count();

            //------------------------User Likes END


            try
            {
                ViewBag.amountContributedPerc = Math.Floor((userProjectById.AmountContributed * 100) / userProjectById.BudgetGoal);
            }
            catch (Exception ex)
            {
                ViewBag.amountContributedPerc = 0;
            }
            




            ViewBag.UserName = userProfile.FullName;
            ViewBag.UserImage = userProfile.ProfilePictureUrl;

            ViewBag.CurrentUserId = User.Identity.GetUserId();

            return View("Project", userProjectById);

            
        }

        public ActionResult Confirm()
        {
            return View();
        }

        [Authorize]
        public ActionResult Like(int projectId)
        {
            var userId = User.Identity.GetUserId();

            
            //var userLike = db.ProjectLikes.Where(aid => aid.ApplicationUserId == userId).FirstOrDefault();

            var getUserLike = (from e in db.ProjectLikes
                               where e.ApplicationUserId == userId && e.ProjectId == projectId
                               select e).FirstOrDefault();

            var userHasLike = (from e in db.ProjectLikes
                               where e.ApplicationUserId == userId && e.ProjectId == projectId
                               select e).Count();

            //var myProjectId = userLike.ProjectId;

            if (userHasLike == 0)
            {
                ProjectLike PL = new ProjectLike
                {
                    ProjectId = projectId,
                    ApplicationUserId = User.Identity.GetUserId()
                };

                db.ProjectLikes.Add(PL);
                db.SaveChanges();
            }
            else
            {
                db.ProjectLikes.Attach(getUserLike);
                db.ProjectLikes.Remove(getUserLike);
                db.SaveChanges();
            }
            
            return RedirectToAction("Project", "Home", new { id = projectId });
        }

        [Authorize]
        public ActionResult Vote(int projectId)
        {
            var userId = User.Identity.GetUserId();


            //var userLike = db.ProjectLikes.Where(aid => aid.ApplicationUserId == userId).FirstOrDefault();

            var getUserVote = (from e in db.ProjectVotes
                               where e.ApplicationUserId == userId && e.ProjectId == projectId
                               select e).FirstOrDefault();

            var userHasVote = (from e in db.ProjectVotes
                               where e.ApplicationUserId == userId && e.ProjectId == projectId
                               select e).Count();

            if (userHasVote == 0)
            {
                ProjectVote PV = new ProjectVote
                {
                    ProjectId = projectId,
                    ApplicationUserId = User.Identity.GetUserId()
                };

                db.ProjectVotes.Add(PV);
                db.SaveChanges();
            }
            else
            {
                db.ProjectVotes.Attach(getUserVote);
                db.ProjectVotes.Remove(getUserVote);
                db.SaveChanges();
            }

            return RedirectToAction("Project", "Home", new { id = projectId });
        }

        [Authorize]
        public ActionResult getMembers(int id)
        {
            //int id = 1012;

            List<string> appUsers = new List<string>();

            List<UserProfile> members = new List<UserProfile>();

            var model = db.UserJoinedProjects.Where(pid => pid.ProjectId == id).ToList();

            

            foreach(var item in model)
            {
                appUsers.Add(item.ApplicationUserId); 
            }

            foreach(var user in appUsers)
            {
                members.Add(db.UserProfiles.Where(aid => aid.ApplicationUserId == user).FirstOrDefault());
            }

            

            return PartialView(members);
        }

        [Authorize]
        public ActionResult ERFs()
        {
            ViewBag.PublishKey = ConfigurationManager.AppSettings["stripePublishableKey"];

            var model = db.MERFs.ToList();

            return View(model);
        }

        [Authorize]
        public ActionResult ERF(string id)
        {
            var model = db.MERFs.Where(tit => tit.Title == id).FirstOrDefault();

            Session["erfId"] = id;

            ViewBag.PublishKey = ConfigurationManager.AppSettings["stripePublishableKey"];

            var userId = model.ApplicationUserId;

            var userProfile = db.UserProfiles.Where(aid => aid.ApplicationUserId == userId).FirstOrDefault();

            var userInfo = db.Users.Where(aid => aid.Id == userId).FirstOrDefault();

            ViewBag.UserName = userProfile.FullName;
            ViewBag.UserImage = userProfile.ProfilePictureUrl;
            ViewBag.UserEmail = userInfo.Email;

            return View(model);
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> Invite(FormCollection formCollection)
        {
            //ApplicationUserManager UserManager = new ApplicationUserManager();

            var projectId = int.Parse(formCollection["projectId"]);
            var message = await AccountController.EmailTemplate("ProjectInvite");

            message = message.Replace("@ViewBag.RecEmail", formCollection["txtEmail"]);
            message = message.Replace("@ViewBag.ProjectTitle", formCollection["txtProjectTitle"]);
            message = message.Replace("@ViewBag.ProjectLink", formCollection["txtProjectUrl"]);
            message = message.Replace("@ViewBag.Comments", formCollection["txtComments"]);
            message = message.Replace("@ViewBag.InviterName", formCollection["txtName"]);

            using (MailMessage mm = new MailMessage("postmaster@boonway.org", formCollection["txtEmail"]))
            {
                mm.Subject = "Project Invite";
                mm.Body = message;
                mm.IsBodyHtml = true;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "mail.boonway.org";
                    smtp.EnableSsl = false;
                    NetworkCredential nc = new NetworkCredential("postmaster@boonway.org", "smartway11@");
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = nc;
                    smtp.Port = 8889;
                    smtp.Send(mm);

                    TempData["InviteMessage"] = "Invitation Sent";

                    return RedirectToAction("Project", "Home", new { id = projectId });
                }

            }


            return View();
        }


        [Authorize]
        public ActionResult Donate(int id)
        {
            var project = db.Projects.Find(id);

            Session["projectTitle"] = project.ProjectTitle;
            Session["projectId"] = id;

            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult MakeDonation(Donation donation, FormCollection formCollection)
        {
            ViewBag.PublishKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            ViewBag.AmountDollars = (donation.AmountInCents * 100).ToString();

            return View(donation);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MakeCharge(Donation donation, string stripeEmail, string stripeToken)
        {
            var projectId = (int)Session["projectId"];

            //var entity = db.Projects.Find(projectId);

            //var projectUp = new Project();

            //db.Entry(entity).CurrentValues.SetValues(projectUp.AmountContributed == 25.00m);
            //db.SaveChanges();

            //StripeConfiguration.SetApiKey("sk_test_fRFwHhOcQrBYP870vFxItlqw");

            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = (donation.AmountInCents * 100), //charge in cents
                Description = Session["projectTitle"].ToString(),
                Currency = "usd",
                CustomerId = customer.Id
            });


            var query = from p in db.Projects
                        where p.Id == projectId
                        select p;

            foreach(Project proj in query)
            {
                proj.AmountContributed += donation.AmountInCents;
            }

            db.SaveChanges();

            //StripeConfiguration.SetApiKey(ConfigurationManager.AppSettings["stripeSecretKey"]);
            donation.ProjectId = projectId;
            donation.DonationDate = DateTime.Now;

            db.Donations.Add(donation);

            db.SaveChanges();
            
            return RedirectToAction("Confirmation");
        }

        public ActionResult Confirmation()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult AddComment(FormCollection formCollection)
        {
            var userId = User.Identity.GetUserId();
            var projectId = int.Parse(formCollection["projectId"]);
            var comments = formCollection["txtComment"];

            Comment newComment = new Comment
            {
                ProjectId = projectId,
                ApplicationUserId = userId,
                UserComment = comments
            };

            db.Comments.Add(newComment);
            db.SaveChanges();

            return RedirectToAction("Project", "Home", new { id = projectId });
        }

        [Authorize]
        //pass project id to get comments of the project by id
        public ActionResult getComments(int id)
        {
            var model = db.Comments.Where(pid => pid.ProjectId == id).ToList();
            return PartialView(model);
        }

        [Authorize]
        public ActionResult getCommentProfile(string id)
        {
            var model = db.UserProfiles.Where(aid => aid.ApplicationUserId == id).ToList();
            return PartialView(model);
        }


        #region ERF Donation
        [Authorize]
        public ActionResult DonateERF()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult MakeERFDonation(ERFDonation erfdonation, FormCollection formCollection)
        {
            ViewBag.PublishKey = ConfigurationManager.AppSettings["stripePublishableKey"];
            ViewBag.AmountDollars = (erfdonation.AmountInCents * 100).ToString();

            return View(erfdonation);
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MakeChargeERF(ERFDonation erfdonation, string stripeEmail, string stripeToken)
        {
            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = (erfdonation.AmountInCents * 100), //charge in cents
                Description = "Donation for emergency relief funds",
                Currency = "usd",
                CustomerId = customer.Id
            });
            
            //erfdonation.MERFID = erfId;
            erfdonation.DonationDate = DateTime.Now;

            db.ERFDonations.Add(erfdonation);

            db.SaveChanges();

            return RedirectToAction("Confirmation");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult MakeContribution(FormCollection formCollection, string stripeEmail, string stripeToken)
        {
            var amount = int.Parse(formCollection["selAmount"]);
            var userId = User.Identity.GetUserId();

            var customers = new StripeCustomerService();
            var charges = new StripeChargeService();

            var customer = customers.Create(new StripeCustomerCreateOptions
            {
                Email = stripeEmail,
                SourceToken = stripeToken
            });

            var charge = charges.Create(new StripeChargeCreateOptions
            {
                Amount = (amount * 100), //charge in cents
                Description = "Contribution for emergency relief funds",
                Currency = "usd",
                CustomerId = customer.Id
            });

            //var erfNumberId = int.Parse(formCollection["erfID"]);

            MemberContribution mc = new MemberContribution();

            mc.AmountInCents = amount;
            //mc.MERFID = erfNumberId;
            mc.ApplicationUserId = userId;
            mc.ContributionDate = DateTime.Now;


            db.MemberContributions.Add(mc);
            db.SaveChanges();

            TempData["Message"] = "We Thank you for the contribution.";
            
            return RedirectToAction("ERFs");
        }


        public ActionResult DeleteComment(int id)
        {
            var comment = db.Comments.Find(id);

            db.Comments.Attach(comment);
            db.Comments.Remove(comment);
            db.SaveChanges();

            var projectId = (int)Session["projectId"];

            TempData["JoinMessage"] = "Comment Deleted";

            return RedirectToAction("Project", new { id = projectId });
        }

        #endregion
    }
}