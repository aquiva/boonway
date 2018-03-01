using System;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Boonwayy.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [MaxLength(500)]
        public string StripeCustomerId { get; set; }
        public DateTime ActiveUntil { get; set; }
        public DateTime? CreditCardExpires { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<UserProposal> UserProposals { get; set; }

        public DbSet<Project> Projects { get; set; }

        public DbSet<UserJoinedProject> UserJoinedProjects { get; set; }

        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<ProjectLike> ProjectLikes { get; set; }

        public DbSet<ProjectVote> ProjectVotes { get; set; }

        //Monthly or Yearly contributions: Emergency relief Funds
        public DbSet<MERF> MERFs { get; set; }

        public DbSet<Donation> Donations { get; set; }

        public DbSet<Comment> Comments { get; set; }

        public DbSet<ERFDonation> ERFDonations { get; set; }

        public DbSet<MemberContribution> MemberContributions { get; set; }
    }
}