namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemoveICollUserProj : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Projects", "UserProfile_Id", "dbo.UserProfiles");
            DropIndex("dbo.Projects", new[] { "UserProfile_Id" });
            DropColumn("dbo.Projects", "UserProfile_Id");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Projects", "UserProfile_Id", c => c.Int());
            CreateIndex("dbo.Projects", "UserProfile_Id");
            AddForeignKey("dbo.Projects", "UserProfile_Id", "dbo.UserProfiles", "Id");
        }
    }
}
