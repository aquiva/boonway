namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Projects",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectTitle = c.String(nullable: false),
                        ProjectCoverImgUrl = c.String(),
                        BudgetGoal = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProjectDescription = c.String(nullable: false),
                        YoutubeVideoUrl = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Projects", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.Projects", new[] { "ApplicationUserId" });
            DropTable("dbo.Projects");
        }
    }
}
