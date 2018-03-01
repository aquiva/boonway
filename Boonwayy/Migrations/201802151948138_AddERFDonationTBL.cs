namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddERFDonationTBL : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ERFDonations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        DonatorName = c.String(nullable: false),
                        Email = c.String(nullable: false),
                        City = c.String(nullable: false),
                        State = c.String(),
                        AmountInCents = c.Int(nullable: false),
                        DonationDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .Index(t => t.ProjectId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ERFDonations", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ERFDonations", new[] { "ProjectId" });
            DropTable("dbo.ERFDonations");
        }
    }
}
