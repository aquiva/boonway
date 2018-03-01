namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProposalTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProposals",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        SocialSecurityNumber = c.String(nullable: false),
                        State = c.String(nullable: false),
                        Address = c.String(nullable: false),
                        OfferLetterUrl = c.String(),
                        DriverLicenseUrl = c.String(),
                        IsApproved = c.Boolean(nullable: false),
                        IsSubmitted = c.Boolean(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProposals", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserProposals", new[] { "ApplicationUserId" });
            DropTable("dbo.UserProposals");
        }
    }
}
