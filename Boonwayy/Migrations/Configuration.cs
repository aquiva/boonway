namespace Boonwayy.Migrations
{
    using Boonwayy.Models;
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<Boonwayy.Models.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Boonwayy.Models.ApplicationDbContext context)
        {
            var userStore = new UserStore<ApplicationUser>(context);
            var userManager = new UserManager<ApplicationUser>(userStore);

            if(!context.Users.Any(t => t.UserName == "admin@boonwayy.com"))
            {
                var user = new ApplicationUser
                {
                    UserName = "admin@boonwayy.com",
                    Email = "admin@boonwayy.com",
                    ActiveUntil = DateTime.Now
                };
                userManager.Create(user, "$Safderali110");

                context.Roles.AddOrUpdate(r => r.Name, new IdentityRole { Name = "Admin" });

                context.SaveChanges();

                userManager.AddToRole(user.Id, "Admin");
            }

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data.
        }
    }
}
