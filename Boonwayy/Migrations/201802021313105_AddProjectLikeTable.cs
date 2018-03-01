namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddProjectLikeTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectLikes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ProjectId = c.Int(nullable: false),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Projects", t => t.ProjectId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ProjectId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ProjectLikes", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectLikes", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectLikes", new[] { "ApplicationUserId" });
            DropIndex("dbo.ProjectLikes", new[] { "ProjectId" });
            DropTable("dbo.ProjectLikes");
        }
    }
}
