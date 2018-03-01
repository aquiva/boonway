namespace Boonwayy.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddUserProfileTable : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FullName = c.String(nullable: false),
                        Gender = c.String(nullable: false),
                        Age = c.Int(nullable: false),
                        ProfilePictureUrl = c.String(),
                        Bio = c.String(),
                        ApplicationUserId = c.String(maxLength: 128),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.ApplicationUserId)
                .Index(t => t.ApplicationUserId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.UserProfiles", "ApplicationUserId", "dbo.AspNetUsers");
            DropIndex("dbo.UserProfiles", new[] { "ApplicationUserId" });
            DropTable("dbo.UserProfiles");
        }
    }
}
