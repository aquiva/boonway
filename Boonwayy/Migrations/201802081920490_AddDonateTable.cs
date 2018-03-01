namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddDonateTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Donations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        DonatorName = c.String(),
                        Email = c.String(),
                        City = c.String(),
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
            DropForeignKey("dbo.Donations", "ProjectId", "dbo.Projects");
            DropIndex("dbo.Donations", new[] { "ProjectId" });
            DropTable("dbo.Donations");
        }
    }
}
