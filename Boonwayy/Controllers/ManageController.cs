using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Boonwayy.Models;
using System.IO;
using Stripe;
using Boonwayy.Services;
//using Boonwayy.Models.Subscription;

namespace Boonwayy.Controllers
{
    [Authorize]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private ApplicationDbContext db = new ApplicationDbContext();
        

        #region CustomMethodsUserDashboard

        public ActionResult ERF()
        {
            var model = db.MERFs.ToList();

            return View(model);
        }

        public ActionResult CreateERF()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateERF(MERF merf)
        {
            var userId = User.Identity.GetUserId();

            merf.ApplicationUserId = userId;

            db.MERFs.Add(merf);
            db.SaveChanges();

            return RedirectToAction("ERF", "Manage");
        }

        public ActionResult EditERF(int id)
        {
            var model = db.MERFs.Find(id);

            ViewBag.ERFTitle = model.Title;

            return View(model);
        }

        [HttpPost]
        public ActionResult EditERF(MERF merf, FormCollection formCollection)
        {
            var entity = db.MERFs.Find(merf.Id);

            if(merf.MERFStartDate == null)
            {
                merf.MERFStartDate = entity.MERFStartDate;
            }

            db.Entry(entity).CurrentValues.SetValues(merf);
            db.SaveChanges();

            return RedirectToAction("ERF", "Manage");
        }

        [HttpPost]
        public ActionResult DeleteERF(MERF merf)
        {
            db.MERFs.Attach(merf);
            db.MERFs.Remove(merf);
            db.SaveChanges();

            return RedirectToAction("ERF", "Manage");
        }


        public ActionResult MyProjects()
        {
            var userId = User.Identity.GetUserId();

            var getUserProjects = db.Projects.Where(aid => aid.ApplicationUserId == userId).ToList();

            //var id = getUserProfileId.Id;

            //var userProjects = (from user in db.UserProfiles.Include("Projects")
            //                   where user.Id == 1
            //                   select user).FirstOrDefault();

            return View(getUserProjects);
        }

        //Show a form by project id with values
        public ActionResult EditProject(int id)
        {
            var userId = User.Identity.GetUserId();

            var model = db.Projects.Where(ui => ui.Id == id).SingleOrDefault();

            if(model.ApplicationUserId == userId)
            {
                return View(model);
            }

            return RedirectToAction("MyProjects", "Manage");
            
            // get if a user has voted a project
            // var model = (from v in db.Projects
                       //where v.ApplicationUserId == userId && v.Id == id
                       //select v);

            
        }
        
        [HttpPost]
        public ActionResult EditProject(Project project, ProjectViewModel projectViewModel)
        {
            try
            {
                if (projectViewModel.CoverImageFile.ContentLength > 0)
                {
                    var supportedImgTypes = new[] { "jpg", "png", "bmp", "PNG" };
                    var imgExtension = Path.GetExtension(projectViewModel.CoverImageFile.FileName).Substring(1);

                    if (!supportedImgTypes.Contains(imgExtension))
                    {
                        ViewBag.UploadError = "Invalid Files, Supported file types are .png, .jpg and .bmp";
                        return View();
                    }
                    else
                    {
                        var coverImgFileName = Path.GetFileName(projectViewModel.CoverImageFile.FileName);
                        var coverImgFilePath = Path.Combine(Server.MapPath("~/Content/Files/ProjectCoverImages"), coverImgFileName);
                        projectViewModel.CoverImageFile.SaveAs(coverImgFilePath);

                        project.ProjectCoverImgUrl = coverImgFileName;
                    }
                }
                else
                {
                    //userProfile.ProfilePictureUrl = entity.ProfilePictureUrl;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
            }


            var entity = db.Projects.Find(project.Id);
            project.Country = "United States";

            db.Entry(entity).CurrentValues.SetValues(project);
            db.SaveChanges();

            TempData["ProjEdit"] = "Project Edited Successfully";
            return RedirectToAction("EditProject", "Manage", new { id = project.Id });
        }

        [HttpPost]
        public ActionResult DeleteProject(Project project)
        {
            //var getProjectById = db.Projects.Find(project.Id);

            db.Projects.Attach(project);
            db.Projects.Remove(project);
            db.SaveChanges();

            return RedirectToAction("MyProjects", "Manage");
        }

        public ActionResult UserProfile()
        {
            var userId = User.Identity.GetUserId();
            var model = db.UserProfiles.Where(ui => ui.ApplicationUserId == userId).FirstOrDefault();

            return View(model);
        }

        
        public ActionResult EditProfile()
        {
            var userId = User.Identity.GetUserId();

            var model = db.UserProfiles.Where(ui => ui.ApplicationUserId == userId).FirstOrDefault();

            

            return View(model);
        }

        [HttpPost]
        public ActionResult EditProfile(UserProfile userProfile, UserProfileViewModel userProfileViewModel, FormCollection formCollection)
        {
            var entity = db.UserProfiles.Find(userProfile.Id);

            try
            {
                if (userProfileViewModel.ProfileImageFile.ContentLength > 0)
                {
                    var supportedImgTypes = new[] { "jpg", "png", "bmp", "PNG" };
                    var imgExtension = Path.GetExtension(userProfileViewModel.ProfileImageFile.FileName).Substring(1);

                    if (!supportedImgTypes.Contains(imgExtension))
                    {
                        ViewBag.UploadError = "Invalid Files, Supported file types are .png, .jpg and .bmp";
                        return View();
                    }
                    else
                    {
                        var profileImgFileName = Path.GetFileName(userProfileViewModel.ProfileImageFile.FileName);
                        var profileImgFilePath = Path.Combine(Server.MapPath("~/Content/Files/UserProfileImages"), profileImgFileName);
                        userProfileViewModel.ProfileImageFile.SaveAs(profileImgFilePath);

                        userProfile.ProfilePictureUrl = profileImgFileName;
                    }
                }
                else
                {
                    //userProfile.ProfilePictureUrl = entity.ProfilePictureUrl;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "";
            }

            db.Entry(entity).CurrentValues.SetValues(userProfile);
            db.SaveChanges();
            ViewBag.Message = "Profile Updated";

            return View();

        }

        // returns customer's subscribed plan id like silver_plan, gold_plan and platinum_plan
        private string getCustomerPlanId()
        {
            var customerSubPlanId = string.Empty;

            try
            {
                var appCustomer = db.Users.Find(User.Identity.GetUserId());

                //StripeConfiguration.SetApiKey("sk_test_j2zhjKiiLyIe3guU8J43YLxn");

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(appCustomer.StripeCustomerId);

                var customObj = customer.Subscriptions.Data;

                var customerSubId = string.Empty;

                foreach (var item in customObj)
                {
                    customerSubId = item.Id;
                }

                var subscriptionService = new StripeSubscriptionService();
                StripeSubscription subscription = subscriptionService.Get(customerSubId);

                var subsObj = subscription.Items;

                foreach (var items in subsObj)
                {
                    customerSubPlanId = items.Plan.Id;
                }

                
            }
            catch (StripeException ex)
            {
                customerSubPlanId = "CelebWorld";
            }

            return customerSubPlanId;
        }
        
        // Get Customer Subscription
        public ActionResult MySubscription(MySubscriptionViewModel mySubscriptionViewModel)
        {
            var customerSubPlanId = string.Empty;

            try
            {
                var appCustomer = db.Users.Find(User.Identity.GetUserId());

                //StripeConfiguration.SetApiKey("sk_test_j2zhjKiiLyIe3guU8J43YLxn");

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(appCustomer.StripeCustomerId);

                var customObj = customer.Subscriptions.Data;

                

                var customerSubId = string.Empty;

                foreach (var item in customObj)
                {
                    customerSubId = item.Id;

                    mySubscriptionViewModel.Name = item.StripePlan.Name;

                    mySubscriptionViewModel.Amount = (int)Math.Floor((decimal)item.StripePlan.Amount / 100);
                    mySubscriptionViewModel.Desc = item.StripePlan.StatementDescriptor;

                    mySubscriptionViewModel.Created = item.Created.Value;
                    mySubscriptionViewModel.Renewal = item.CurrentPeriodEnd.Value;
                }

                

                var subscriptionService = new StripeSubscriptionService();
                StripeSubscription subscription = subscriptionService.Get(customerSubId);

                var subsObj = subscription.Items;

                foreach (var items in subsObj)
                {
                    customerSubPlanId = items.Plan.Id;
                }


            }
            catch (StripeException ex)
            {
                customerSubPlanId = "CelebWorld";
            }



            //StripeConfiguration.SetApiKey("sk_test_j2zhjKiiLyIe3guU8J43YLxn");

            //var planService = new StripePlanService();
            //StripePlan plan = planService.Get(getCustomerPlanId());

            //mySubscriptionViewModel.Id = plan.Id;
            //mySubscriptionViewModel.Name = plan.Name;
            //mySubscriptionViewModel.Interval = plan.Interval;
            //mySubscriptionViewModel.Amount = (int)Math.Floor((decimal)plan.Amount / 100);
            //mySubscriptionViewModel.Desc = plan.StatementDescriptor;

            //mySubscriptionViewModel.Created = plan.Created;

            //return (int)Math.Floor((decimal)this.AmountInCents / 100);

            return View(mySubscriptionViewModel);
        }

        public ActionResult CancelSubscription()
        {
            try
            {
                var appCustomer = db.Users.Find(User.Identity.GetUserId());

                //StripeConfiguration.SetApiKey("sk_test_j2zhjKiiLyIe3guU8J43YLxn");

                var customerService = new StripeCustomerService();
                StripeCustomer customer = customerService.Get(appCustomer.StripeCustomerId);

                var customObj = customer.Subscriptions.Data;

                var customerSubId = string.Empty;

                foreach (var item in customObj)
                {
                    customerSubId = item.Id;
                }

                var subscriptionService = new StripeSubscriptionService();
                subscriptionService.Cancel(customerSubId);

                return RedirectToAction("Index", "Manage");
            }
            catch(Exception ex)
            {

            }

            return View();        
        }

        //Show user proposal application form
        public ActionResult CreateUserProposal()
        {
            try
            {
                var userId = User.Identity.GetUserId();

                var userInformation = db.UserProposals.Where(ui => ui.ApplicationUserId == userId).FirstOrDefault();

                if (userId == userInformation.ApplicationUserId && userInformation.IsSubmitted == true && userInformation.IsApproved == false)
                {
                    ViewBag.Message = "Your application is in review process";
                }
                else if (userId == userInformation.ApplicationUserId && userInformation.IsSubmitted == true && userInformation.IsApproved == true)
                {
                    ViewBag.Message = "Your application has been approved, Now you can subscribe to our plans and join projects other than CelebWorld.";
                }
                else
                {
                    // Do nothing
                }

                return View();
            }
            catch (Exception ex)
            {
                return View();
            }
            
        }

        //Show project posting form
        public ActionResult PostProject()
        {
            //var userId = User.Identity.GetUserId();
            //var userInformation = db.UserProposals.Where(ui => ui.ApplicationUserId == userId).FirstOrDefault();

            //try
            //{
            //    if (userInformation.IsSubmitted == true && userInformation.IsApproved == true)
            //    {
            //        return View();
            //    }
            //    else
            //    {
            //        return RedirectToAction("CreateUserProposal", "Manage");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    if(ex.Message == "Object reference not set to an instance of an object.")
            //    {
            //        return RedirectToAction("CreateUserProposal", "Manage");
            //    }
                
            //}
            

            return View();
        }

        [HttpPost]
        public ActionResult PostProject(Project project, ProjectViewModel projectViewModel, FormCollection formCollection)
        {
            var customerProjectCategory = string.Empty;

            if (formCollection["postInCW"] != null)
            {
                customerProjectCategory = "CelebWorld";
            }
            else
            {
                customerProjectCategory = getCustomerPlanId();
            }

            project.ProjectCategory = customerProjectCategory;
            project.ApplicationUserId = User.Identity.GetUserId();

            if(projectViewModel.CoverImageFile.ContentLength > 0)
            {
                var supportedImgTypes = new[] { "jpg", "png", "bmp", "PNG" };
                var imgExtension = Path.GetExtension(projectViewModel.CoverImageFile.FileName).Substring(1);

                if (!supportedImgTypes.Contains(imgExtension))
                {
                    ViewBag.UploadError = "Invalid Files, Supported file types are .png, .jpg and .bmp";
                    return View();
                }
                else
                {
                    var coverImgFileName = Path.GetFileName(projectViewModel.CoverImageFile.FileName);
                    var coverImgFilePath = Path.Combine(Server.MapPath("~/Content/Files/ProjectCoverImages"), coverImgFileName);
                    projectViewModel.CoverImageFile.SaveAs(coverImgFilePath);

                    project.ProjectCoverImgUrl = coverImgFileName;

                }
            }

            //project.YoutubeVideoUrl.Replace(".com/watch?v=", ".com/embed/");

            //var userId = User.Identity.GetUserId();

            //var getUserProfileId = db.UserProfiles.Where(aid => aid.ApplicationUserId == userId).FirstOrDefault();

            

            project.ProjectStartDate = DateTime.Now.Date;
            project.Country = "United States";

            db.Projects.Add(project);
            db.SaveChanges();

            var lastInsertedId = db.Projects.Find(project.Id);

            int projId = lastInsertedId.Id;

            return RedirectToAction("Project", "Home", new { id = projId });
        }

        public ActionResult Project(int id)
        {
            var userProjectById = db.Projects.Find(id);

            ViewBag.CurrentUserId = User.Identity.GetUserId();
            
            return View(userProjectById);
        }

        [HttpPost]
        public ActionResult JoinProject(FormCollection formCollection)
        {
            var customerPlanId = getCustomerPlanId();

            var project_category = formCollection["project_category"];
            var project_id = formCollection["project_id"];

            int projId = int.Parse(project_id);

            var userId = User.Identity.GetUserId();

            if (customerPlanId == project_category || project_category == "CelebWorld")
            {
                //Check if user is already joined the project
                var userProject = db.UserJoinedProjects.Any(ui => ui.ProjectId == projId);

                var select = (from uj in db.UserJoinedProjects
                              where uj.ApplicationUserId == userId && uj.ProjectId == projId
                              select uj).Any();
             
                if (select)
                {
                    TempData["JoinMessage"] = "You are already a member of the project";

                    return RedirectToAction("Project", "Home", new { id = projId });
                }
                else
                {
                    UserJoinedProject userJoinedProject = new UserJoinedProject();
                    userJoinedProject.ApplicationUserId = userId;
                    userJoinedProject.ProjectId = int.Parse(project_id);

                    db.UserJoinedProjects.Add(userJoinedProject);
                    db.SaveChanges();

                    TempData["JoinMessage"] = "You have successfully joined the project.";

                    return RedirectToAction("Project", "Home", new { id = projId });
                }

                
            }
            
           
            TempData["JoinMessage"] = "Your subscription plan does not meet requirements to join the project.";
            return RedirectToAction("Project", "Home", new { id = projId });
        }

        //Create User Proposal Application with file uploads
        [HttpPost]
        public ActionResult CreateUserProposal(UserProposal userProposal, UserProposalViewModel userProposalViewModel)
        {
            if (userProposalViewModel.offerLetterFile.ContentLength > 0 && userProposalViewModel.driverLicenseFile.ContentLength > 0)
            {

                var supportedDocTypes = new[] { "doc", "docx", "pdf" };
                var supportedImgTypes = new[] { "jpg", "png", "bmp", "PNG" };

                var docExtension = Path.GetExtension(userProposalViewModel.offerLetterFile.FileName).Substring(1);
                var imgExtension = Path.GetExtension(userProposalViewModel.driverLicenseFile.FileName).Substring(1);

                if (!supportedDocTypes.Contains(docExtension) || !supportedImgTypes.Contains(imgExtension))
                {
                    ViewBag.UploadError = "Invalid Files, Supported file types are .doc, .docx, .pdf, .png, .jpg";
                    return View();
                }
                //10485760 = 10MB
                else if (userProposalViewModel.offerLetterFile.ContentLength > 10485760 || userProposalViewModel.driverLicenseFile.ContentLength > 10485760)
                {
                    ViewBag.UploadError = "Maximum File Size Exceeded, 10MB is allowed";
                }
                else
                {
                    var olFileName = Path.GetFileName(userProposalViewModel.offerLetterFile.FileName);
                    var olPath = Path.Combine(Server.MapPath("~/Content/Files/OfferLetters"), olFileName);
                    userProposalViewModel.offerLetterFile.SaveAs(olPath);

                    var dlFileName = Path.GetFileName(userProposalViewModel.driverLicenseFile.FileName);
                    var dlPath = Path.Combine(Server.MapPath("~/Content/Files/DriverLicenses"), dlFileName);
                    userProposalViewModel.driverLicenseFile.SaveAs(dlPath);

                    userProposal.OfferLetterUrl = olFileName;
                    userProposal.DriverLicenseUrl = dlFileName;

                    userProposal.ApplicationUserId = User.Identity.GetUserId();
                    userProposal.IsApproved = false;
                    userProposal.IsSubmitted = true;
                    

                    db.UserProposals.Add(userProposal);
                    db.SaveChanges();

                    //ViewBag.Message = "Your application is in review process.";

                    return RedirectToAction("CreateUserProposal", "Manage");
                }

            }


            return View();
        }

        #endregion

        public ManageController()
        {
        }

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager)
        {
           
            UserManager = userManager;
            SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set 
            { 
                _signInManager = value; 
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public async Task<ActionResult> Index(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
                : message == ManageMessageId.Error ? "An error has occurred."
                : message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
                : message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
                : "";

            var userId = User.Identity.GetUserId();
            var model = new IndexViewModel
            {
                HasPassword = HasPassword(),
                PhoneNumber = await UserManager.GetPhoneNumberAsync(userId),
                TwoFactor = await UserManager.GetTwoFactorEnabledAsync(userId),
                Logins = await UserManager.GetLoginsAsync(userId),
                BrowserRemembered = await AuthenticationManager.TwoFactorBrowserRememberedAsync(userId)
            };
            return View(model);
        }

        //
        // POST: /Manage/RemoveLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
        {
            ManageMessageId? message;
            var result = await UserManager.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("ManageLogins", new { Message = message });
        }

        //
        // GET: /Manage/AddPhoneNumber
        public ActionResult AddPhoneNumber()
        {
            return View();
        }

        //
        // POST: /Manage/AddPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            // Generate the token and send it
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), model.Number);
            if (UserManager.SmsService != null)
            {
                var message = new IdentityMessage
                {
                    Destination = model.Number,
                    Body = "Your security code is: " + code
                };
                await UserManager.SmsService.SendAsync(message);
            }
            return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
        }

        //
        // POST: /Manage/EnableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EnableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), true);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // POST: /Manage/DisableTwoFactorAuthentication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DisableTwoFactorAuthentication()
        {
            await UserManager.SetTwoFactorEnabledAsync(User.Identity.GetUserId(), false);
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", "Manage");
        }

        //
        // GET: /Manage/VerifyPhoneNumber
        public async Task<ActionResult> VerifyPhoneNumber(string phoneNumber)
        {
            var code = await UserManager.GenerateChangePhoneNumberTokenAsync(User.Identity.GetUserId(), phoneNumber);
            // Send an SMS through the SMS provider to verify the phone number
            return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
        }

        //
        // POST: /Manage/VerifyPhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePhoneNumberAsync(User.Identity.GetUserId(), model.PhoneNumber, model.Code);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Failed to verify phone");
            return View(model);
        }

        //
        // POST: /Manage/RemovePhoneNumber
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> RemovePhoneNumber()
        {
            var result = await UserManager.SetPhoneNumberAsync(User.Identity.GetUserId(), null);
            if (!result.Succeeded)
            {
                return RedirectToAction("Index", new { Message = ManageMessageId.Error });
            }
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
            }
            return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
        }

        //
        // GET: /Manage/ChangePassword
        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        //
        // GET: /Manage/SetPassword
        public ActionResult SetPassword()
        {
            return View();
        }

        //
        // POST: /Manage/SetPassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await UserManager.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                if (result.Succeeded)
                {
                    var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
                    if (user != null)
                    {
                        await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                    }
                    return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
                }
                AddErrors(result);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Manage/ManageLogins
        public async Task<ActionResult> ManageLogins(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            var user = await UserManager.FindByIdAsync(User.Identity.GetUserId());
            if (user == null)
            {
                return View("Error");
            }
            var userLogins = await UserManager.GetLoginsAsync(User.Identity.GetUserId());
            var otherLogins = AuthenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
            ViewBag.ShowRemoveButton = user.PasswordHash != null || userLogins.Count > 1;
            return View(new ManageLoginsViewModel
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        //
        // POST: /Manage/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "Manage"), User.Identity.GetUserId());
        }

        //
        // GET: /Manage/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
            }
            var result = await UserManager.AddLoginAsync(User.Identity.GetUserId(), loginInfo.Login);
            return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _userManager != null)
            {
                _userManager.Dispose();
                _userManager = null;
            }

            base.Dispose(disposing);
        }

#region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

#endregion
    }
}