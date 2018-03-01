namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddVoteTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ProjectVotes",
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
            DropForeignKey("dbo.ProjectVotes", "ApplicationUserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.ProjectVotes", "ProjectId", "dbo.Projects");
            DropIndex("dbo.ProjectVotes", new[] { "ApplicationUserId" });
            DropIndex("dbo.ProjectVotes", new[] { "ProjectId" });
            DropTable("dbo.ProjectVotes");
        }
    }
}
