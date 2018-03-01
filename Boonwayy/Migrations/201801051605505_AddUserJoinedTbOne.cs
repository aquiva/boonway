namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserJoinedTbOne : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserJoinedProjects",
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
            DropForeignKey("dbo.UserJoinedProjects", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserJoinedProjects", "ProjectId", "dbo.Projects");
            DropIndex("dbo.UserJoinedProjects", new[] { "ApplicationUserId" });
            DropIndex("dbo.UserJoinedProjects", new[] { "ProjectId" });
            DropTable("dbo.UserJoinedProjects");
        }
    }
}
